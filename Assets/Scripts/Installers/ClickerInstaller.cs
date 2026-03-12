using QCore.Tools.Providers;
using Services.Clicker;
using Services.Clicker.Configs;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ClickerInstaller : MonoInstaller
    {
        [SerializeField] private ClickerConfig _clickerConfig;
        
        public override void InstallBindings()
        {
            DefaultDataProvider<ClickerConfig> clickerConfigProvider = new DefaultDataProvider<ClickerConfig>(_clickerConfig);
            
            Container.Bind<IDataProvider<ClickerConfig>>().FromInstance(clickerConfigProvider).AsSingle();
            Container.Bind<IClickerService>().To<ClickerService>().AsSingle();
            Container.Bind<AutoClicker>().AsSingle();
            Container.Bind<EnergyRecover>().AsSingle();
        }
    }
}