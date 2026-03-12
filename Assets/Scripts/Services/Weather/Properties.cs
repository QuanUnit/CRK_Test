using System;
using System.Collections.Generic;

namespace Services.Weather
{
    [Serializable]
    public struct Properties
    {
        public List<Period> Periods;
    }
}