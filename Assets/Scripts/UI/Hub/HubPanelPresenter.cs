using System.Linq;
using QCore.UISystem.Panels;
using QCore.UISystem.Presenters;
using QCore.UISystem.Services;
using Zenject;

namespace UI.Hub
{
    public class HubPanelPresenter : PanelPresenter<HubPanel>
    {
        [Inject] private IUIPanelsService _panelsService;

        private Panel _openedTab;
        
        protected override void StartPresentingInternal()
        {
            Panel.TabButtonClicked += TabClickHandle;
            
            OpenTab(Panel.TabButtons.First().AttachedPanel);
        }

        private void TabClickHandle(Panel panel)
        {
            OpenTab(panel);
        }

        private void OpenTab(Panel panel)
        {
            if(_openedTab == panel) return;

            CloseActiveTab();
            
            PanelAndPresenterData panelData = _panelsService.Open(() => panel);
            _openedTab = panelData.Panel;
        }

        private void CloseActiveTab()
        {
            if (_openedTab != null)
            {
                _panelsService.Close(_openedTab.GetType());
            }

            _openedTab = null;
        }

        protected override void StopPresentingInternal()
        {
            Panel.TabButtonClicked -= TabClickHandle;
        }
    }
}