using System;
using Newtonsoft.Json;
using Services.RestAPI;
using UnityEngine;
using Zenject;

namespace Services.Breeds
{
    public class BreedsListService : IBreedsListService
    {
        private const string Url = "https://dogapi.dog/api/v2/breeds?page%5Bnumber%5D={0}&page%5Bsize%5D={1}";

        [Inject] private IHttpRequestsService _httpRequestsService;

        private Guid _activeRequestId;

        public void FetchBreeds(int page = 1, int size = 15, Action<BreedsDataList> onSuccess = null)
        {
            string url = string.Format(Url, page, size);

            _activeRequestId = _httpRequestsService.Get(url, onSuccess: (id, json) =>
            {
                RemoveActiveRequest(id);
                BreedsDataList breeds = ParseData(json);
                onSuccess?.Invoke(breeds);
            }, onError: (id, error) =>
            {
                RemoveActiveRequest(id);
            }, onCancel: id =>
            {
                RemoveActiveRequest(id);
            });
        }

        private BreedsDataList ParseData(string json)
        {
            return JsonConvert.DeserializeObject<BreedsDataList>(json);
        }

        private void RemoveActiveRequest(Guid requestId)
        {
            if (requestId == _activeRequestId)
            {
                _activeRequestId = Guid.Empty;
            }
        }

        public void StopFetching()
        {
            _httpRequestsService.CancelRequest(_activeRequestId);
            RemoveActiveRequest(_activeRequestId);
        }
    }
}