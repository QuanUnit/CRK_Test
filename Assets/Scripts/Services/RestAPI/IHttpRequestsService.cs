using System;
using System.Collections.Generic;

namespace Services.RestAPI
{
    public interface IHttpRequestsService
    {
        public Guid Get(string url, RequestSuccessHandler onSuccess = null, RequestErrorHandler onError = null, RequestCancelHandler onCancel = null, Dictionary<string, string> headers = null);
        public Guid Post(string url, string jsonData, RequestSuccessHandler onSuccess = null, RequestErrorHandler onError = null, RequestCancelHandler onCancel = null, Dictionary<string, string> headers = null);
        public Guid Put(string url, string jsonData, RequestSuccessHandler onSuccess = null, RequestErrorHandler onError = null, RequestCancelHandler onCancel = null, Dictionary<string, string> headers = null);
        public Guid Delete(string url, RequestSuccessHandler onSuccess = null, RequestErrorHandler onError = null, RequestCancelHandler onCancel = null, Dictionary<string, string> headers = null);
        public Guid GetTexture(string url, TextureRequestSuccessHandler onSuccess = null, RequestErrorHandler onError = null, RequestCancelHandler onCancel = null, Dictionary<string, string> headers = null);
        
        public bool CancelRequest(Guid requestId);
        public void CancelAllRequests();
        public RequestStatus GetRequestStatus(Guid requestId);
        public int GetQueueLength();
        public bool IsProcessing();
        public void Dispose();
    }

    public enum RequestStatus
    {
        NotFound,
        InQueue,
        InProgress
    }
}