using System;
using UnityEngine;
using UnityEngine.UI;

namespace Services.Audio.Components
{
    [RequireComponent(typeof(Button))]
    public class AudioPlayerButtonClick : AudioPlayerMonoComponent
    {
        [SerializeField] private Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(Play);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Play);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            _button ??= GetComponent<Button>();
        }

#endif
    }
}