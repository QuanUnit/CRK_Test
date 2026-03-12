using QCore.StateMachine.States;
using QCore.Tools.Extensions;
using QCore.UISystem.Services;
using UI.Hub;
using Zenject;

namespace App.Flow.States
{
    public class PlayingSessionState : State
    {
        [Inject] private IUIPanelsService _uiPanelsService;

        private HubPanel _panel;
        
        public override void Enter()
        {
            PanelAndPresenterData<HubPanel> panelData = _uiPanelsService.Open<HubPanel>(useAnimation: true);
            _panel = panelData.Panel;
        }

        public override void Exit()
        {
            if (_panel.IsNullOrDestroyed() == false)
            {
                _uiPanelsService.Close<HubPanel>();
            }
        }
    }
}