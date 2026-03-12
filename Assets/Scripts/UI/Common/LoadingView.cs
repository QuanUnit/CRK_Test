using UnityEngine;

namespace UI.Common
{
    public class LoadingView : MonoBehaviour
    {
        [SerializeField] private float _animtionSpeed = 200;
        [SerializeField] private Transform _rotatingTarget;

        private void Update()
        {
            _rotatingTarget.Rotate(Vector3.forward * (_animtionSpeed * Time.deltaTime));
        }
    }
}