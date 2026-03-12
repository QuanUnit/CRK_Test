using System;
using QCore.UISystem.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    [RequireComponent(typeof(Button))]
    public class TabButton : MonoBehaviour
    {
        public event Action<Panel> Clicked;
        public Panel AttachedPanel => _attachedPanel;
            
        [SerializeField] private Button _button;
        [SerializeField] private Panel _attachedPanel;

        private void OnEnable()
        {
            _button.onClick.AddListener(ClickHandle);
        }

        private void ClickHandle()
        {
            Clicked?.Invoke(_attachedPanel);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ClickHandle);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            _button ??= GetComponent<Button>();
        }

#endif
    }
}