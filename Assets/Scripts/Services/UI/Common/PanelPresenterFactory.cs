using System;
using System.Collections.Generic;
using QCore.UISystem.Panels;
using QCore.UISystem.Presenters;
using QCore.UISystem.Services.Factories;
using UI.Hub;
using UI.Hub.BreedsPanel;
using UI.Hub.ClickerPanel;
using UI.Hub.WeatherPanel;
using Zenject;

namespace Services.UI.Common
{
    public class PanelPresenterFactory : IPanelPresenterFactory
    {
        [Inject] private DiContainer _container;

        private readonly Dictionary<Type, Type> _panelTypeToPresenterType = new Dictionary<Type, Type>()
        {
            { typeof(HubPanel), typeof(HubPanelPresenter) },
            { typeof(ClickerPanel), typeof(ClickerPanelPresenter) },
            { typeof(WeatherPanel), typeof(WeatherPanelPresenter) },
            { typeof(BreedsPanel), typeof(BreedsPanelPresenter) },
        };
        
        public IPanelPresenter<T> Create<T>(T panel) where T : Panel
        {
            return (IPanelPresenter<T>)Create((Panel)panel);
        }

        public IPanelPresenter Create(Panel panel)
        {
            Type panelType = panel.GetType();
            IPanelPresenter presenter = CreateInternal(panelType);
            return presenter;
        }

        private IPanelPresenter CreateInternal(Type panelType)
        {
            Type presenterType = _panelTypeToPresenterType[panelType];

            return (IPanelPresenter)_container.Instantiate(presenterType);
        }
    }
}