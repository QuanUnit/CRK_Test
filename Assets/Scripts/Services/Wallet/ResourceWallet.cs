namespace Services.Wallet
{
    public class ResourceWallet
    {
        public event WalletTransactionHandler Changed;

        public int Amount => _amount;

        private int _amount;
        
        public ResourceWallet(int amount)
        {
            _amount = amount;
        }

        public bool CanSpend(int amount)
        {
            return ValidateSpend(amount);
        }

        public bool CanDeposit(int amount)
        {
            return ValidateDeposit(amount);
        }
        
        public bool TrySpend(int amount)
        {
            if (ValidateSpend(amount) == false) return false;

            SubtractResource(ref _amount, amount);
            Changed?.Invoke(this, Amount, amount);
            return true;
        }

        public bool TryDeposit(int amount)
        {
            if (ValidateDeposit(amount) == false) return false;

            DepositResource(ref _amount, amount);
            Changed?.Invoke(this, Amount, amount);
            return true;
        }

        protected virtual void SubtractResource(ref int source, int amount)
        {
            source -= amount;
        }

        protected virtual void DepositResource(ref int source, int amount)
        {
            source += amount;
        }

        protected virtual bool ValidateSpend(int amount)
        {
            if (amount < 0) return false;
            if (Amount < amount) return false;

            return true;
        }

        protected virtual bool ValidateDeposit(int amount)
        {
            if (amount < 0) return false;

            return true;
        }
    }

    public delegate void WalletTransactionHandler(ResourceWallet sender, int currentAmount, int delta);
}