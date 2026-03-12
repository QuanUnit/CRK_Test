using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI.Hub.ClickerPanel
{
    public class ResourceParticle : MonoBehaviour
    {
        [SerializeField] private float _throwHeight;
        [SerializeField] private float _throwDuration;
        [SerializeField] private Ease _throwEase;

        [SerializeField] private float _fadeDelay;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private Ease _fadeEase;
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _resourceText;
        [SerializeField] private string _currencyFormat = "+{0}";

        private Sequence _sequence;
        
        public void SetAmount(int amount)
        {
            _resourceText.text = string.Format(_currencyFormat, amount);
        }

        public void Play(Action finishCallback = default)
        {
            Kill();
            
            _canvasGroup.alpha = 1;
            _sequence = DOTween.Sequence();
            
            _sequence
                .Join(GetThrowTween())
                .Join(GetFadeTween())
                .OnComplete(() => finishCallback?.Invoke());
        }

        private Tween GetThrowTween()
        {
            return transform.DOLocalMoveY(_throwHeight, _throwDuration).SetEase(_throwEase).SetRelative();
        }

        private Tween GetFadeTween()
        {
            return _canvasGroup.DOFade(0f, _fadeDuration).SetEase(_fadeEase).SetDelay(_fadeDelay);
        }

        public void Kill()
        {
            if (_sequence != null)
            {
                _sequence.Kill();
                _sequence = null;
            }
        }
    }
}