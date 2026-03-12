using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Services.RestAPI;
using Zenject;

namespace Services.Breeds
{
    public class BreedDescriptionService : IBreedDescriptionService
    {
        [Inject] private IHttpRequestsService _httpRequestsService;

        private const string Url = "https://dogapi.dog/api/v2/breeds/{0}";
        
        private Guid _activeRequestId;
        private Guid _fetchingBreedId;

        public void Fetch(Guid breedId, Action<BreedData> onSuccess = null, Action<Guid, string> onError = null, Action<Guid> onCancel = null)
        {
            string url = string.Format(Url, breedId);

            _fetchingBreedId = breedId;
            _activeRequestId = _httpRequestsService.Get(url, onSuccess: (id, json) =>
            {
                RemoveActiveRequest(id);
                BreedData breed = ParseData(json).Data;
                onSuccess?.Invoke(breed);
            }, onError: (id, error) =>
            {
                RemoveActiveRequest(id);
                onError?.Invoke(breedId, error);
            }, onCancel: id =>
            {
                RemoveActiveRequest(id);
                onCancel?.Invoke(breedId);
            });
        }

        public bool IsFetchingBreed(Guid breedId)
        {
            return _fetchingBreedId == breedId;
        }

        private void RemoveActiveRequest(Guid requestId)
        {
            if (requestId == _activeRequestId)
            {
                _activeRequestId = Guid.Empty;
                _fetchingBreedId = Guid.Empty;
            }
        }

        private BreedDataResponse ParseData(string json)
        {
            return JsonConvert.DeserializeObject<BreedDataResponse>(json);
        }

        public void StopFetching()
        {
            _httpRequestsService.CancelRequest(_activeRequestId);
            RemoveActiveRequest(_activeRequestId);
        }

        [Serializable]
        private struct BreedDataResponse
        {
            public BreedData Data;
        }
    }
}