using System;

namespace Services.Breeds
{
    [Serializable]
    public struct BreedData
    {
        public Guid Id;
        public string Type;
        public BreedAttributes Attributes;
    }
}