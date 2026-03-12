using UnityEngine;

namespace Services.Clicker.Configs
{
    [CreateAssetMenu(menuName = "Configs/Clicker/" + nameof(ClickerConfig), fileName = nameof(ClickerConfig))]
    public class ClickerConfig : ScriptableObject
    {
        public int EnergyConsumptionPerClick => _energyConsumptionPerClick;
        public int SoftCurrencyRewardPerClick => _softCurrencyRewardPerClick;
        public float AutoClickInterval => _autoClickInterval;
        public int EnergyRecoverAmountPerTick => _energyRecoverAmountPerTick;
        public int EnergyRecoverInterval => _energyRecoverInterval;
        
        [SerializeField] private int _energyConsumptionPerClick;
        [SerializeField] private int _softCurrencyRewardPerClick;
        [SerializeField] private float _autoClickInterval;

        [SerializeField] private int _energyRecoverAmountPerTick;
        [SerializeField] private int _energyRecoverInterval;
    }
}