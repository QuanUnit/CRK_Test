using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hub.BreedsPanel
{
    public class BreedDescriptionPopup : MonoBehaviour
    {
        public event Action ClaimClicked;
        
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private Button _claimButton;

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void SetView(string title, string description)
        {
            _title.text = title;
            _description.text = description;
        }

        private void OnEnable()
        {
            _claimButton.onClick.AddListener(ClaimClickHandle);
        }

        private void ClaimClickHandle()
        {
            ClaimClicked?.Invoke();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _claimButton.onClick.RemoveListener(ClaimClickHandle);
        }
    }
}