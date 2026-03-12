using UnityEngine;

namespace Services.Wallet.Configs
{
    [CreateAssetMenu(menuName = "Configs/Wallet/" + nameof(ClampedResourceWalletConfig), fileName = nameof(ClampedResourceWalletConfig))]
    public class ClampedResourceWalletConfig : ResourceWalletConfig
    {
        public int MaxAmount => _maxAmount;
        
        [SerializeField] private int _maxAmount;
    }
}