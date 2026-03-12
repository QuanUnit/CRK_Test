using System;

namespace Services.Weather
{
    [Serializable]
    public struct Period
    {
        public int Number;
        public string Name;
        public float Temperature;
        public string TemperatureUnit;
        public string ShortForecast;
        public string DetailedForecast;
        public string Icon;
        public string WindSpeed;
        public string WindDirection;
    }
}