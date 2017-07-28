using System;

namespace WaterUseAgent.Resources
{
    public class Configuration
    {
        public bool HasPermits { get; set; }
        public Int32 MinYear { get; set; }
        public Int32 MaxYear { get; set; }
        public bool HasReturns { get; set; }
        public string Units { get; set; }
    }
}
