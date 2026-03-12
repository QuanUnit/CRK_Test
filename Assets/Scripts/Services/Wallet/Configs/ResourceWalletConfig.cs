using UnityEngine;

namespace Services.Wallet.Configs
{
    [CreateAssetMenu(menuName = "Configs/Wallet/" + nameof(ResourceWalletConfig), fileName = nameof(ResourceWalletConfig))]
    public class ResourceWalletConfig : ScriptableObject
    {
        public int Amount => _amount;
        
        [SerializeField] private int _amount;
    }
}