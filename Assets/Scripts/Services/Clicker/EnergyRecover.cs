using QCore.Tools.Providers;
using QCore.Tools.Timers;
using Services.Clicker.Configs;
using Services.Wallet;
using Zenject;

namespace Services.Clicker
{
    public class EnergyRecover
    {
        private ClickerConfig Config => _configProvider.Provide();
        
        [Inject] private EnergyWallet _energyWallet;
        [Inject] private IDataProvider<ClickerConfig> _configProvider;

        private AsyncLooper _looper;
        
        public void Launch()
        {
            _looper ??= new AsyncLooper(Config.EnergyRecoverInterval);
            _looper.Start(TickHandle);
        }

        private void TickHandle()
        {
            _energyWallet.TryDeposit(Config.EnergyRecoverAmountPerTick);
        }

        public void Stop()
        {
            _looper.Stop();
        }
    }
}