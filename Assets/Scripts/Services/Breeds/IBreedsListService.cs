using System;

namespace Services.Breeds
{
    public interface IBreedsListService
    {
        public void FetchBreeds(int page = 1, int size = 15, Action<BreedsDataList> onSuccess = null);
        public void StopFetching();
    }
}