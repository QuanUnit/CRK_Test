using Services.Breeds;
using Zenject;

namespace Installers
{
    public class BreedsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IBreedsListService>().To<BreedsListService>().AsSingle();
            Container.Bind<IBreedDescriptionService>().To<BreedDescriptionService>().AsSingle();
        }
    }
}