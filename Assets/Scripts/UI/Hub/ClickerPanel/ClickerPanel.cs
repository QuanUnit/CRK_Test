using QCore.UISystem.Panels;
using UI.Wallets;
using UnityEngine;

namespace UI.Hub.ClickerPanel
{
    public class ClickerPanel : Panel
    {
        public ResourceWalletView SoftCurrencyView => _softCurrencyView;
        public ClampedResourceWalletView EnergyWalletView => _energyWalletView;
        public ClickerParticlesThrower ClickerParticlesThrower => _clickerParticlesThrower;
        public ClickerButton ClickerButton => _clickerButton;
        
        [SerializeField] private ResourceWalletView _softCurrencyView;
        [SerializeField] private ClampedResourceWalletView _energyWalletView;
        [SerializeField] private ClickerParticlesThrower _clickerParticlesThrower;
        [SerializeField] private ClickerButton _clickerButton;
    }
}