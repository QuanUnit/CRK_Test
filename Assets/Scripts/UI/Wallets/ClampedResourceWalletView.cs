using TMPro;
using UnityEngine;

namespace UI.Wallets
{
    public class ClampedResourceWalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private string _format = "{0}/{1}";
        
        public void SetAmount(int amount, int maxAmount)
        {
            _amountText.text = string.Format(_format, amount, maxAmount);
        }
    }
}