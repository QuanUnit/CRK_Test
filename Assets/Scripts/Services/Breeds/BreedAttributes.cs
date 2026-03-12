using System;

namespace Services.Breeds
{
    [Serializable]
    public struct BreedAttributes
    {
        public string Name;
        public string Description;
        public LifeSpan Life;
        public WeightRange MaleWeight;
        public WeightRange FemaleWeight;
        public bool Hypoallergenic;
    }
}