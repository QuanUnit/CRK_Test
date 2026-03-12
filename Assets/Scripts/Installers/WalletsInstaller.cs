using Services.Wallet;
using Services.Wallet.Configs;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class WalletsInstaller : MonoInstaller
    {
        [SerializeField] private ResourceWalletConfig _softCurrencyWalletConfig;
        [SerializeField] private ClampedResourceWalletConfig _energyWalletConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<EnergyWallet>().AsSingle().WithArguments(_energyWalletConfig.Amount, _energyWalletConfig.MaxAmount);
            Container.Bind<SoftCurrencyWallet>().AsSingle().WithArguments(_softCurrencyWalletConfig.Amount);
        }
    }
}