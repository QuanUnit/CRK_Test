using UnityEngine;
using Zenject;

namespace App.Flow
{
    public class Bootstrapper : MonoBehaviour
    {
        [Inject] private AppStateMachineFactory _appStateMachineFactory;

        private AppStateMachine _appStateMachine;
        
        private void Start()
        {
            _appStateMachine = _appStateMachineFactory.Create();
            _appStateMachine.EnterFirstState();
        }

        private void OnDestroy()
        {
            _appStateMachine?.Dispose();
        }
    }
}