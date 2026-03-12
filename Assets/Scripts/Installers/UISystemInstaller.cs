using QCore.UISystem.Services;
using QCore.UISystem.Services.Factories;
using QCore.UISystem.Services.PanelsSources;
using QCore.UISystem.Services.PanelsSources.Configs;
using Services.UI.Common;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UISystemInstaller : MonoInstaller
    {
        [SerializeField] private PersistentPanelsPrefabsSourceConfig _panelsSourceConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_panelsSourceConfig).AsSingle();
            
            Container.Bind<IPanelsPrefabsSource>().To<PersistentPanelsPrefabsSource>().AsSingle();
            Container.Bind<IPanelPresenterFactory>().To<PanelPresenterFactory>().AsSingle();
            Container.Bind<IPanelFactory>().To<PanelFactory>().AsSingle();
            Container.Bind<IUIPanelsService>().To<UIPanelsService>().AsSingle();
        }
    }
}