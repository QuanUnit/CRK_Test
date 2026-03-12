using QCore.StateMachine.States;
using QCore.UISystem.Services;
using Services.Audio;
using Services.Clicker;
using Zenject;

namespace App.Flow.States
{
    public class BootstrapState : State
    {
        [Inject] private IUIPanelsService _panelsService;
        [Inject] private EnergyRecover _energyRecover;
        [Inject] private IAudioService _audioService;

        public override void Enter()
        {
            InitServices();
            StateMachine.EnterState<PlayingSessionState>();
        }

        private void InitServices()
        {
            _panelsService.Init();
            _audioService.Init();
            _energyRecover.Launch();
        }
    }
}