using System;
using QCore.Tools.Providers;
using QCore.Tools.Timers;
using Services.Clicker.Configs;
using Zenject;

namespace Services.Clicker
{
    public class AutoClicker
    {
        public event Action AutoClickRequested;
        private ClickerConfig Config => _configProvider.Provide();
        
        [Inject] private IDataProvider<ClickerConfig> _configProvider;
        
        private AsyncLooper _looper;
        
        public void Launch()
        {
            _looper ??= new AsyncLooper(Config.AutoClickInterval);
            _looper.Start(TickHandle);
        }

        private void TickHandle()
        {
            AutoClickRequested?.Invoke();
        }

        public void Stop()
        {
            _looper.Stop();
        }
    }
}