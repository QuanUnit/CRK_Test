using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hub.ClickerPanel
{
    [RequireComponent(typeof(Button))]
    public class ClickerButton : MonoBehaviour
    {
        public event Action Clicked;

        [SerializeField] private Button _button;
        [SerializeField] private ClickerButtonAnimation _animation;

        private void OnEnable()
        {
            _button.onClick.AddListener(ClickHandle);
        }

        private void ClickHandle()
        {
            _animation.PlayClick();
            Clicked?.Invoke();
        }

        public void SimulateClick(bool sendEvent = true)
        {
            _animation.PlayClick();

            if (sendEvent)
            {
                Clicked?.Invoke();
            }
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