using Services.RestAPI;
using Zenject;

namespace Installers
{
    public class RestAPIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IHttpRequestsService>().To<HttpRequestsService>().AsSingle();
        }
    }
}