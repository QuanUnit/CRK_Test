using System;
using TMPro;
using UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hub.BreedsPanel
{
    public class BreedView : MonoBehaviour
    {
        public event Action<BreedView> Clicked;
        
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private LoadingView _loadingView;

        private void OnEnable()
        {
            _button.onClick.AddListener(ClickHandle);
            ToggleLoading(false);
        }

        public void SetView(int number, string name)
        {
            _number.text = number.ToString();
            _name.text = name;
        }
        
        public void ToggleLoading(bool value)
        {
            _loadingView.gameObject.SetActive(value);
        }

        private void ClickHandle()
        {
            Clicked?.Invoke(this);
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveListener(ClickHandle);
        }
    }
}