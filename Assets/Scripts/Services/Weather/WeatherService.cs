using System;
using System.Linq;
using Newtonsoft.Json;
using QCore.Tools.Timers;
using Services.RestAPI;
using Zenject;

namespace Services.Weather
{
    public class WeatherService : IWeatherService
    {
        [Inject] private IHttpRequestsService _httpRequestsService;

        private const string Url = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        private Guid _activeRequestId;
        private Guid _activeIconRequestId;
        private AsyncLooper _looper;
        private Action<WeatherData> _successCallback;

        public void StartFetching(Action<WeatherData> onSuccess)
        {
            _successCallback = onSuccess;

            TickHandle();
            _looper ??= new AsyncLooper(5);
            _looper.Start(TickHandle);
        }

        private void TickHandle()
        {
            TryAppendRequest();
        }

        private bool TryAppendRequest()
        {
            if (_activeRequestId != Guid.Empty) return false;

            _activeRequestId = _httpRequestsService.Get(Url,
                onSuccess: (id, json) =>
                {
                    WeatherResponseData weatherResponseData = ParseWeatherData(json);
                    DownloadWeatherIcon(weatherResponseData);
                },
                onCancel: id =>
                {
                    ClearActiveRequestId(id);
                },
                onError: (id, error) =>
                {
                    ClearActiveRequestId(id);
                });

            return true;
        }

        private void DownloadWeatherIcon(WeatherResponseData weatherResponseData)
        {
            Period firstPeriod = weatherResponseData.Properties.Periods.First();
            WeatherData weatherData = ConstructResultWeatherData(firstPeriod);

            _activeIconRequestId = _httpRequestsService.GetTexture(firstPeriod.Icon,
                onSuccess: (id, texture) =>
                {
                    weatherData.IconTexture = texture;

                    _successCallback?.Invoke(weatherData);

                    ClearActiveRequestId(_activeRequestId);
                    ClearActiveIconRequestId(id);
                },
                onError: (id, error) =>
                {
                    ClearActiveRequestId(_activeRequestId);
                    ClearActiveIconRequestId(id);
                },
                onCancel: id =>
                {
                    ClearActiveRequestId(_activeRequestId);
                    ClearActiveIconRequestId(id);
                }
            );
        }

        private WeatherResponseData ParseWeatherData(string json)
        {
            WeatherResponseData weather = JsonConvert.DeserializeObject<WeatherResponseData>(json);
            return weather;
        }

        private WeatherData ConstructResultWeatherData(Period period)
        {
            return new WeatherData()
            {
                IconTexture = null,
                Temperature = period.Temperature,
            };
        }

        private void ClearActiveRequestId(Guid requestId)
        {
            if (_activeRequestId == requestId)
            {
                _activeRequestId = Guid.Empty;
            }
        }

        private void ClearActiveIconRequestId(Guid requestId)
        {
            if (_activeIconRequestId == requestId)
            {
                _activeIconRequestId = Guid.Empty;
            }
        }

        public void StopFetching()
        {
            _looper.Stop();

            _httpRequestsService.CancelRequest(_activeRequestId);
            ClearActiveRequestId(_activeRequestId);
            _httpRequestsService.CancelRequest(_activeIconRequestId);
            ClearActiveIconRequestId(_activeIconRequestId);
        }
    }
}