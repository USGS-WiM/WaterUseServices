//------------------------------------------------------------------------------
//----- Resource ---------------------------------------------------------------
//------------------------------------------------------------------------------

//-------1---------2---------3---------4---------5---------6---------7---------8
//       01234567890123456789012345678901234567890123456789012345678901234567890
//-------+---------+---------+---------+---------+---------+---------+---------+

// copyright:   2017 WiM - USGS

//    authors:  Jeremy K. Newson USGS Web Informatics and Mapping
//              
//  
//   purpose:   Simple Plain Old Class Object (POCO) 
//
//discussion:   POCO's arn't derived from special base classed nor do they return any special types for their properties.
//              
//
//   

using NpgsqlTypes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WaterUseDB.Resources
{
    public class Source
    {
        [Required]
        public int ID { get; set; }
        public string Name { get; set; }
        [Required]
        public string FacilityName { get; set; }
        public string StationID { get; set; }
        [Required]
        public int CatagoryTypeID { get; set; }
        [Required]
        public int SourceTypeID { get; set; }
        [Required]
        public int RegionID { get; set; }
        [Required]
        public PostgisPoint Location { get; set; }

        public Region Region { get; set; }
        public CatagoryType CatagoryType { get; set; }
        public SourceType SourceType { get; set; }
        public List<Permit> Permits { get; set; }
        public List<TimeSeries> TimeSeries { get; set; }
    }
}
