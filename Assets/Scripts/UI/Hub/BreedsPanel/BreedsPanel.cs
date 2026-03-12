using System;
using System.Collections.Generic;
using System.Linq;
using QCore.Tools.GameObjectsPool;
using QCore.UISystem.Panels;
using Services.Breeds;
using UI.Common;
using UnityEngine;

namespace UI.Hub.BreedsPanel
{
    public class BreedsPanel : Panel
    {
        public event Action<Guid> BreedViewClicked;
        public BreedDescriptionPopup BreedDescriptionPopup => _breedDescriptionPopup;
        
        [SerializeField] private BreedView _breedViewPrefab;
        [SerializeField] private Transform _breedsViewport;
        [SerializeField] private LoadingView _loadingView;

        [SerializeField] private int _breedsPersistentPoolSize;
        [SerializeField] private BreedDescriptionPopup _breedDescriptionPopup;
        
        private GameObjectPool<BreedView> _breedViewsPool;
        private Dictionary<Guid, BreedView> _activeBreedViews = new Dictionary<Guid, BreedView>();
        
        protected override void Awake()
        {
            base.Awake();
            
            _breedViewsPool = new GameObjectPool<BreedView>(_breedViewPrefab, _breedsPersistentPoolSize, InstantiateBreedView);
            _breedViewsPool.InitRoot(transform);
            _breedViewsPool.WarmUp();
        }

        private void OnEnable()
        {
            _loadingView.gameObject.SetActive(true);
        }

        public void CreateBreedViews(IEnumerable<BreedData> breedViews)
        {
            int i = 0;
            
            foreach (var breedData in breedViews)
            {
                i++;
                
                BreedView breedView = _breedViewsPool.Get(_breedsViewport);
                breedView.Clicked += BreedViewClickHandle;
                breedView.SetView(i, breedData.Attributes.Name);
                _activeBreedViews.Add(breedData.Id, breedView);
            }

            if (i > 0)
            {
                _loadingView.gameObject.SetActive(false);
            }
        }

        private void BreedViewClickHandle(BreedView sender)
        {
            Guid breedId = _activeBreedViews.First(a => a.Value == sender).Key;
            BreedViewClicked?.Invoke(breedId);
        }

        private BreedView InstantiateBreedView(BreedView prefab, Transform root)
        {
            return Instantiate(prefab, root);
        }

        public void ToggleLoadingForBreedView(Guid breedId, bool value)
        {
            if (_activeBreedViews.TryGetValue(breedId, out BreedView breedView) == false) return;
            
            breedView.ToggleLoading(value);
        }

        public void Clear()
        {
            foreach (var breedView in _activeBreedViews.Values)
            {
                breedView.Clicked -= BreedViewClickHandle;
            }
            
            _activeBreedViews.Clear();
            _breedViewsPool.ReleaseAllElements();
        }
    }
}