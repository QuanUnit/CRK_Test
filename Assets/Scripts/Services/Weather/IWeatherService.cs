using System;

namespace Services.Weather
{
    public interface IWeatherService
    {
        public void StartFetching(Action<WeatherData> onSuccess);
        public void StopFetching();
    }
}