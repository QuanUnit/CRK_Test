using App.Flow;
using ExternalDependencies.Zenject.Source.Util;
using Zenject;

namespace Installers
{
    public class AppFlowInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AppStateMachineFactory>().AsSingle();
            
            DiContextScope.SetContext(Container);
        }
    }
}