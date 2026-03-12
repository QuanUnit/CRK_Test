using Services.Weather;
using Zenject;

namespace Installers
{
    public class WeatherInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IWeatherService>().To<WeatherService>().AsSingle();
        }
    }
}