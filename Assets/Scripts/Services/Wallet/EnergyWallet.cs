namespace Services.Wallet
{
    public class EnergyWallet : ClampedResourceWallet
    {
        public EnergyWallet(int amount, int maxAmount) : base(amount, maxAmount)
        {
        }
    }
}