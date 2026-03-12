using QCore.StateMachine.States;
using Services.Audio;
using Services.Clicker;
using Services.RestAPI;
using Zenject;

namespace App.Flow.States
{
    public class CleaningState : State
    {
        [Inject] private EnergyRecover _energyRecover;
        [Inject] private IHttpRequestsService _httpRequestsService;
        [Inject] private IAudioService _audioService;

        public override void Enter()
        {
            DisposeServices();
        }

        private void DisposeServices()
        {
            _energyRecover.Stop();
            _httpRequestsService.Dispose();
            _audioService.Dispose();
        }
    }
}