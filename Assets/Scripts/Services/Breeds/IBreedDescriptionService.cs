using System;

namespace Services.Breeds
{
    public interface IBreedDescriptionService
    {
        public void Fetch(Guid breedId, Action<BreedData> onSuccess = null, Action<Guid, string> onError = null, Action<Guid> onCancel = null);
        public bool IsFetchingBreed(Guid breedId);
        public void StopFetching();
    }
}   