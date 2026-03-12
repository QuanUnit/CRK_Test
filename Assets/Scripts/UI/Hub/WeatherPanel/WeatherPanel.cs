using QCore.UISystem.Panels;
using Services.Weather;
using UI.Common;
using UnityEngine;

namespace UI.Hub.WeatherPanel
{
    public class WeatherPanel : Panel
    {
        [SerializeField] private WeatherPlateView _weatherPlateView;
        [SerializeField] private LoadingView _loadingView;

        private void OnEnable()
        {
            _weatherPlateView.gameObject.SetActive(false);
            _loadingView.gameObject.SetActive(true);
        }

        public void SetWeatherPlateView(WeatherData weatherData)
        {
            _loadingView.gameObject.SetActive(false);
            _weatherPlateView.gameObject.SetActive(true);
            _weatherPlateView.SetView(weatherData.Temperature, weatherData.IconTexture);
        }
    }
}