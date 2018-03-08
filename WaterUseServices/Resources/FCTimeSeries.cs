using System;
using System.ComponentModel.DataAnnotations;
using WaterUseDB.Resources;
namespace WaterUseServices.Resources
{
    public class FCTimeSeries:IFacilityCode
    {
        [Required]
        public string FacilityCode { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public double Value { get; set; }
        [Required]
        public int UnitTypeID { get; set; }
    }
}
