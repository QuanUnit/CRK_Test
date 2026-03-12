using System;

namespace Services.Clicker
{
    public interface IClickerService
    {
        public event Action<int> ClickExecuted;
        public bool TryExecuteClick();
    }
}