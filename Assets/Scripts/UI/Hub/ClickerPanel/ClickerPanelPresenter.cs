using QCore.UISystem.Presenters;
using Services.Clicker;
using Services.Wallet;
using Zenject;

namespace UI.Hub.ClickerPanel
{
    public class ClickerPanelPresenter : PanelPresenter<ClickerPanel>
    {
        [Inject] private IClickerService _clickerService;
        [Inject] private EnergyWallet _energyWallet;
        [Inject] private SoftCurrencyWallet _softCurrencyWallet;
        [Inject] private AutoClicker _autoClicker;
        
        protected override void StartPresentingInternal()
        {
            _energyWallet.Changed += EnergyWalletChangeHandle;
            _softCurrencyWallet.Changed += SoftCurrencyWalletChangeHandle;
            _clickerService.ClickExecuted += ClickHandle;
            
            _autoClicker.AutoClickRequested += AutoClickRequestHandle;
            Panel.ClickerButton.Clicked += ButtonClickHandle;
            
            _autoClicker.Launch();
            
            UpdateEnergyView();
            UpdateSoftCurrencyView();
        }

        private void AutoClickRequestHandle()
        {
            bool clickExecuted = _clickerService.TryExecuteClick();

            if (clickExecuted)
            {
                Panel.ClickerButton.SimulateClick(false);
            }
        }

        private void ClickHandle(int amount)
        {
            Panel.ClickerParticlesThrower.Throw(amount);
        }

        private void SoftCurrencyWalletChangeHandle(ResourceWallet sender, int currentAmount, int delta)
        {
            UpdateSoftCurrencyView();
        }

        private void EnergyWalletChangeHandle(ResourceWallet sender, int currentAmount, int delta)
        {
            UpdateEnergyView();
        }

        private void UpdateEnergyView()
        {
            Panel.EnergyWalletView.SetAmount(_energyWallet.Amount, _energyWallet.MaxAmount);
        }

        private void UpdateSoftCurrencyView()
        {
            Panel.SoftCurrencyView.SetAmount(_softCurrencyWallet.Amount);
        }
        
        private void ButtonClickHandle()
        {
            _clickerService.TryExecuteClick();
        }

        protected override void StopPresentingInternal()
        {
            _energyWallet.Changed -= EnergyWalletChangeHandle;
            _softCurrencyWallet.Changed -= SoftCurrencyWalletChangeHandle;
            _clickerService.ClickExecuted -= ClickHandle;
            _autoClicker.AutoClickRequested -= AutoClickRequestHandle;

            Panel.ClickerButton.Clicked -= ButtonClickHandle;
            Panel.ClickerParticlesThrower.Clear();

            _autoClicker.Stop();
        }
    }
}