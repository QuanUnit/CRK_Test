using System;
using System.Collections.Generic;
using QCore.UISystem.Panels;
using UI.Common;
using UnityEngine;

namespace UI.Hub
{
    public class HubPanel : Panel
    {
        public event Action<Panel> TabButtonClicked;
        public IReadOnlyList<TabButton> TabButtons => _tabButtons;

        [SerializeField] private List<TabButton> _tabButtons;

        private void OnEnable()
        {
            foreach (var button in _tabButtons)
            {
                button.Clicked += TabButtonClickHandle;
            }
        }

        private void TabButtonClickHandle(Panel attachedPanel)
        {
            TabButtonClicked?.Invoke(attachedPanel);
        }

        private void OnDisable()
        {
            foreach (var button in _tabButtons)
            {
                button.Clicked -= TabButtonClickHandle;
            }
        }
    }
}