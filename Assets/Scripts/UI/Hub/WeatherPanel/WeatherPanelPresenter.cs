using QCore.UISystem.Presenters;
using Services.Weather;
using Zenject;

namespace UI.Hub.WeatherPanel
{
    public class WeatherPanelPresenter : PanelPresenter<WeatherPanel>
    {
        [Inject] private IWeatherService _weatherService;
        
        protected override void StartPresentingInternal()
        {
            _weatherService.StartFetching(WeatherDataGetHandle);
        }

        private void WeatherDataGetHandle(WeatherData weatherData)
        {
            Panel.SetWeatherPlateView(weatherData);
        }

        protected override void StopPresentingInternal()
        {
            _weatherService.StopFetching();
        }
    }
}