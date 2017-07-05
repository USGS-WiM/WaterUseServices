using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterUseAgent.Resources
{
    public class Wateruse
    {
        public Int32 StartYear { get; set; }
        public Int32? EndYear { get; set; }
        public bool ShouldSerializeEndYear()
        { return EndYear.HasValue; }
        public DateTime ProcessDate { get; set; }
        public WateruseSummary Return { get; set; }
        public WateruseSummary Withdrawal { get; set; }
    }

    public class WateruseSummary {
        public WateruseValue Annual { get; set; }
        public IDictionary<Int32, MonthlySummary> Monthly { get; set; }
        public PermittedSummary Permitted { get; set; }
        public bool ShouldSerializePermitted()
        { return Permitted != null; }
    }
    public class MonthlySummary {
        public WateruseValue Month { get; set; }
        public IDictionary<string,WateruseValue> Code { get; set; }
        public bool ShouldSerializeCode()
        { return Code != null && Code.Count > 0; }
    }

    public class PermittedSummary {
        public WateruseValue Well { get; set; }
        public WateruseValue Intake { get; set; }
    }

    public class WateruseValue {
        public string Name { get; set; }
        public double Value { get; set; }
        public object Unit { get; set; }
        public string Description { get; set; }
    }
}
