using QCore.Tools.GameObjectsPool;
using UnityEngine;

namespace Services.Wallet
{
    public class ClampedResourceWallet : ResourceWallet
    {
        public bool IsFull => MaxAmount == Amount;
        public int MaxAmount { get; private set; }
        
        public ClampedResourceWallet(int amount, int maxAmount) : base(amount)
        {
            MaxAmount = maxAmount;
        }

        protected override bool ValidateDeposit(int amount)
        {
            return base.ValidateDeposit(amount) && IsFull == false;
        }

        protected override void DepositResource(ref int source, int amount)
        {
            source += amount;
            source = Mathf.Clamp(source, 0, MaxAmount);
        }
    }
}