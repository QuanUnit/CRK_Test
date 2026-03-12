using App.Flow.States;
using QCore.StateMachine.States;
using Zenject;

namespace App.Flow
{
    public class AppStateMachineFactory
    {
        [Inject] private DiContainer _container;
        
        public AppStateMachine Create()
        {
            AppStateMachine stateMachine = new AppStateMachine();
            
            stateMachine.AddState(CreateState<BootstrapState>());
            stateMachine.AddState(CreateState<PlayingSessionState>());
            stateMachine.AddState(CreateState<CleaningState>());

            return stateMachine;
        }

        private T CreateState<T>() where T : IExitableState
        {
            T result = _container.Instantiate<T>();
            return result;
        }
    }
}