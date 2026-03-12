using UnityEngine;

namespace UI.Hub.ClickerPanel
{
    public class ClickerButtonAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private readonly int _clickTrigger = Animator.StringToHash("Click");
        
        public void PlayClick()
        {
            _animator.SetTrigger(_clickTrigger);
        }
    }
}