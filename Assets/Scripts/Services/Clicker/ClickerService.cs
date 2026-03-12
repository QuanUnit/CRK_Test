using System;
using QCore.Tools.Providers;
using Services.Clicker.Configs;
using Services.Wallet;
using Zenject;

namespace Services.Clicker
{
    public class ClickerService : IClickerService
    {
        public event Action<int> ClickExecuted;
        private ClickerConfig Config => _configProvider.Provide();

        [Inject] private SoftCurrencyWallet _softCurrencyWallet;
        [Inject] private EnergyWallet _energyWallet;
        [Inject] private IDataProvider<ClickerConfig> _configProvider;

        public bool TryExecuteClick()
        {
            int energyConsumption = Config.EnergyConsumptionPerClick;
            int softCurrencyReward = Config.SoftCurrencyRewardPerClick;

            if (_energyWallet.CanSpend(energyConsumption) == false) return false;
            if (_softCurrencyWallet.CanDeposit(softCurrencyReward) == false) return false;
            
            _energyWallet.TrySpend(energyConsumption);
            _softCurrencyWallet.TryDeposit(softCurrencyReward);

            ClickExecuted?.Invoke(softCurrencyReward);
            
            return true;
        }
    }
}