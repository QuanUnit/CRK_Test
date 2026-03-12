using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Services.RestAPI
{
    public class HttpRequestsService : IHttpRequestsService
    {
        private readonly Queue<QueuedRequest> _requestQueue = new Queue<QueuedRequest>();
        private readonly Dictionary<Guid, QueuedRequest> _pendingRequests = new Dictionary<Guid, QueuedRequest>();
        private readonly CancellationTokenSource _serviceCts = new CancellationTokenSource();
        private QueuedRequest _currentRequest;
        private bool _isProcessing;
        private UniTask _processingTask;

        public Guid Get(string url, RequestSuccessHandler onSuccess = null, RequestErrorHandler onError = null,
            RequestCancelHandler onCancel = null, Dictionary<string, string> headers = null)
        {
            return AddJsonRequest(url, "GET", null, headers, onSuccess, onError, onCancel);
        }

        public Guid Post(string url, string jsonData, RequestSuccessHandler onSuccess = null,
            RequestErrorHandler onError = null, RequestCancelHandler onCancel = null,
            Dictionary<string, string> headers = null)
        {
            return AddJsonRequest(url, "POST", jsonData, headers, onSuccess, onError, onCancel);
        }

        public Guid Put(string url, string jsonData, RequestSuccessHandler onSuccess = null,
            RequestErrorHandler onError = null, RequestCancelHandler onCancel = null,
            Dictionary<string, string> headers = null)
        {
            return AddJsonRequest(url, "PUT", jsonData, headers, onSuccess, onError, onCancel);
        }

        public Guid Delete(string url, RequestSuccessHandler onSuccess = null, RequestErrorHandler onError = null,
            RequestCancelHandler onCancel = null, Dictionary<string, string> headers = null)
        {
            return AddJsonRequest(url, "DELETE", null, headers, onSuccess, onError, onCancel);
        }

        public Guid GetTexture(string url, TextureRequestSuccessHandler onSuccess = null,
            RequestErrorHandler onError = null, RequestCancelHandler onCancel = null,
            Dictionary<string, string> headers = null)
        {
            Guid requestId = Guid.NewGuid();
            QueuedRequest request = new QueuedRequest(requestId, url, headers, onSuccess, onError, onCancel);

            _requestQueue.Enqueue(request);
            _pendingRequests[requestId] = request;

            TryStartProcessing();

            return requestId;
        }

        private Guid AddJsonRequest(string url, string method, string jsonData,
            Dictionary<string, string> headers,
            RequestSuccessHandler onSuccess, RequestErrorHandler onError,
            RequestCancelHandler onCancel)
        {
            Guid requestId = Guid.NewGuid();
            QueuedRequest request =
                new QueuedRequest(requestId, url, method, jsonData, headers, onSuccess, onError, onCancel);

            _requestQueue.Enqueue(request);
            _pendingRequests[requestId] = request;

            TryStartProcessing();

            return requestId;
        }

        private void TryStartProcessing()
        {
            if (!_isProcessing)
            {
                _isProcessing = true;
                _processingTask = ProcessQueueAsync();
            }
        }

        private async UniTask ProcessQueueAsync()
        {
            try
            {
                while (_requestQueue.Count > 0)
                {
                    QueuedRequest request;
                    request = _requestQueue.Dequeue();

                    _currentRequest = request;

                    if (request.CancellationTokenSource.Token.IsCancellationRequested)
                    {
                        CompleteRequest(request);
                        continue;
                    }

                    await SendRequestAsync(request);
                }
            }
            finally
            {
                _isProcessing = false;
                _currentRequest = null;
            }
        }

        private async UniTask SendRequestAsync(QueuedRequest request)
        {
            try
            {
                if (request.RequestType == RequestType.Texture)
                {
                    await SendTextureRequestAsync(request);
                }
                else
                {
                    await SendJsonRequestAsync(request);
                }
            }
            catch (OperationCanceledException)
            {
                request.OnCancel?.Invoke(request.Id);
            }
            catch (Exception ex)
            {
                request.OnError?.Invoke(request.Id, $"Exception: {ex.Message}");
            }
            finally
            {
                CompleteRequest(request);
            }
        }

        private async UniTask SendJsonRequestAsync(QueuedRequest request)
        {
            using UnityWebRequest webRequest = CreateWebRequest(request);

            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    webRequest.SetRequestHeader(header.Key, header.Value);
                }
            }

            if (!string.IsNullOrEmpty(request.JsonData))
            {
                webRequest.SetRequestHeader("Content-Type", "application/json");
            }

            await webRequest.SendWebRequest()
                .WithCancellation(request.CancellationTokenSource.Token);

            if (request.CancellationTokenSource.Token.IsCancellationRequested)
            {
                request.OnCancel?.Invoke(request.Id);
                return;
            }

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                request.OnSuccessJson?.Invoke(request.Id, webRequest.downloadHandler.text);
            }
            else
            {
                request.OnError?.Invoke(request.Id, $"Error: {webRequest.error} - Code: {webRequest.responseCode}");
            }
        }

        private async UniTask SendTextureRequestAsync(QueuedRequest request)
        {
            using UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(request.Url);

            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    webRequest.SetRequestHeader(header.Key, header.Value);
                }
            }

            await webRequest.SendWebRequest()
                .WithCancellation(request.CancellationTokenSource.Token);

            if (request.CancellationTokenSource.Token.IsCancellationRequested)
            {
                request.OnCancel?.Invoke(request.Id);
                return;
            }

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                request.OnSuccessTexture?.Invoke(request.Id, texture);
            }
            else
            {
                request.OnError?.Invoke(request.Id, $"Error loading texture: {webRequest.error} - Code: {webRequest.responseCode}");
            }
        }

        private void CompleteRequest(QueuedRequest request)
        {
            request.IsCompleted = true;
            request.CancellationTokenSource.Dispose();
            _pendingRequests.Remove(request.Id);
        }

        public bool CancelRequest(Guid requestId)
        {
            if (requestId == Guid.Empty) return false;

            if (_pendingRequests.TryGetValue(requestId, out QueuedRequest request))
            {
                if (!request.IsCompleted)
                {
                    request.CancellationTokenSource.Cancel();
                    return true;
                }
            }

            return false;
        }

        public void CancelAllRequests()
        {
            _serviceCts.Cancel();

            foreach (var request in _pendingRequests.Values)
            {
                if (!request.IsCompleted)
                {
                    request.CancellationTokenSource.Cancel();
                }
            }

            _pendingRequests.Clear();
            _requestQueue.Clear();

            _isProcessing = false;
            _currentRequest = null;
        }

        public RequestStatus GetRequestStatus(Guid requestId)
        {
            if (requestId == Guid.Empty)
            {
                return RequestStatus.NotFound;
            }

            if (!_pendingRequests.TryGetValue(requestId, out QueuedRequest request))
            {
                return RequestStatus.NotFound;
            }

            if (request.IsCompleted)
            {
                return RequestStatus.NotFound;
            }

            return _currentRequest?.Id == requestId ? RequestStatus.InProgress : RequestStatus.InQueue;
        }

        public int GetQueueLength()
        {
            return _requestQueue.Count;
        }

        public bool IsProcessing()
        {
            return _isProcessing;
        }

        private UnityWebRequest CreateWebRequest(QueuedRequest request)
        {
            string method = request.Method.ToUpperInvariant();

            return method switch
            {
                "GET" => BuildGetRequest(request.Url),
                "POST" => BuildPostRequest(request),
                "PUT" => BuildPutRequest(request),
                "DELETE" => BuildDeleteRequest(request.Url),
                _ => BuildGetRequest(request.Url)
            };
        }

        private UnityWebRequest BuildGetRequest(string url)
        {
            return UnityWebRequest.Get(url);
        }

        private UnityWebRequest BuildPostRequest(QueuedRequest request)
        {
            return string.IsNullOrEmpty(request.JsonData)
                ? BuildEmptyBodyRequest(request.Url, "POST")
                : UnityWebRequest.PostWwwForm(request.Url, request.JsonData);
        }

        private UnityWebRequest BuildPutRequest(QueuedRequest request)
        {
            if (string.IsNullOrEmpty(request.JsonData))
            {
                return BuildEmptyBodyRequest(request.Url, "PUT");
            }

            UnityWebRequest putRequest = new UnityWebRequest(request.Url, "PUT")
            {
                downloadHandler = new DownloadHandlerBuffer(),
                uploadHandler = new UploadHandlerRaw(
                    System.Text.Encoding.UTF8.GetBytes(request.JsonData)
                )
            };

            return putRequest;
        }

        private UnityWebRequest BuildDeleteRequest(string url)
        {
            return UnityWebRequest.Delete(url);
        }

        private UnityWebRequest BuildEmptyBodyRequest(string url, string method)
        {
            return new UnityWebRequest(url, method)
            {
                downloadHandler = new DownloadHandlerBuffer()
            };
        }

        public void Dispose()
        {
            _serviceCts.Cancel();
            _serviceCts.Dispose();
            CancelAllRequests();
        }

        private enum RequestType
        {
            Json,
            Texture
        }

        private class QueuedRequest
        {
            public Guid Id { get; }
            public string Url { get; }
            public string Method { get; }
            public string JsonData { get; }
            public Dictionary<string, string> Headers { get; }
            public RequestSuccessHandler OnSuccessJson { get; }
            public TextureRequestSuccessHandler OnSuccessTexture { get; }
            public RequestErrorHandler OnError { get; }
            public RequestCancelHandler OnCancel { get; }
            public CancellationTokenSource CancellationTokenSource { get; }
            public bool IsCompleted { get; set; }
            public RequestType RequestType { get; }

            public QueuedRequest(Guid id, string url, string method, string jsonData,
                Dictionary<string, string> headers,
                RequestSuccessHandler onSuccess, RequestErrorHandler onError,
                RequestCancelHandler onCancel)
            {
                Id = id;
                Url = url;
                Method = method;
                JsonData = jsonData;
                Headers = headers;
                OnSuccessJson = onSuccess;
                OnError = onError;
                OnCancel = onCancel;
                CancellationTokenSource = new CancellationTokenSource();
                RequestType = RequestType.Json;
            }

            public QueuedRequest(Guid id, string url,
                Dictionary<string, string> headers,
                TextureRequestSuccessHandler onSuccess, RequestErrorHandler onError,
                RequestCancelHandler onCancel)
            {
                Id = id;
                Url = url;
                Headers = headers;
                OnSuccessTexture = onSuccess;
                OnError = onError;
                OnCancel = onCancel;
                CancellationTokenSource = new CancellationTokenSource();
                RequestType = RequestType.Texture;
            }
        }
    }

    public delegate void RequestSuccessHandler(Guid requestId, string json);

    public delegate void TextureRequestSuccessHandler(Guid requestId, Texture2D texture);

    public delegate void RequestErrorHandler(Guid requestId, string error);

    public delegate void RequestCancelHandler(Guid requestId);
}