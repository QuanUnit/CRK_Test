using System;
using QCore.UISystem.Presenters;
using Services.Breeds;
using Zenject;

namespace UI.Hub.BreedsPanel
{
    public class BreedsPanelPresenter : PanelPresenter<BreedsPanel>
    {
        [Inject] private IBreedsListService _breedsListService;
        [Inject] private IBreedDescriptionService _breedDescriptionService;
        
        protected override void StartPresentingInternal()
        {
            _breedsListService.FetchBreeds(onSuccess: BreedsListFetchHandle);

            Panel.BreedViewClicked += BreedClickHandle;
            Panel.BreedDescriptionPopup.ClaimClicked += BreedDescriptionPopupClaimHandle;
        }

        private void BreedClickHandle(Guid breedId)
        {
            if(_breedDescriptionService.IsFetchingBreed(breedId)) return;
            
            Panel.ToggleLoadingForBreedView(breedId, true);

            _breedDescriptionService.StopFetching();
            _breedDescriptionService.Fetch(breedId, BreedDescriptionFetchHandle, BreedDescriptionFetchErrorHandle, BreedDescriptionFetchCancelHandle);
        }

        private void BreedDescriptionFetchCancelHandle(Guid breedId)
        {
            Panel.ToggleLoadingForBreedView(breedId, false);
        }

        private void BreedDescriptionFetchErrorHandle(Guid breedId, string error)
        {
            Panel.ToggleLoadingForBreedView(breedId, false);
        }

        private void BreedDescriptionFetchHandle(BreedData breedData)
        {
            Panel.ToggleLoadingForBreedView(breedData.Id, false);
            Panel.BreedDescriptionPopup.Show();
            Panel.BreedDescriptionPopup.SetView(breedData.Attributes.Name, breedData.Attributes.Description);
        }

        private void BreedsListFetchHandle(BreedsDataList breedsDataList)
        {
            Panel.CreateBreedViews(breedsDataList.Data);
        }
        
        private void BreedDescriptionPopupClaimHandle()
        {
            Panel.BreedDescriptionPopup.Hide();
        }

        protected override void StopPresentingInternal()
        {
            Panel.BreedDescriptionPopup.Hide();
            Panel.BreedDescriptionPopup.ClaimClicked -= BreedDescriptionPopupClaimHandle;
            Panel.BreedViewClicked -= BreedClickHandle;
            _breedsListService.StopFetching();
            _breedDescriptionService.StopFetching();
            
            Panel.Clear();
        }
    }
}