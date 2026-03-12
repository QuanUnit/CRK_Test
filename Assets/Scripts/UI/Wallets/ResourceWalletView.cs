using TMPro;
using UnityEngine;

namespace UI.Wallets
{
    public class ResourceWalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountText;

        public void SetAmount(int amount)
        {
            _amountText.text = amount.ToString();
        }
    }
}