﻿//------------------------------------------------------------------------------
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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WaterUseDB.Resources
{
    public partial class Region
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }
        public string Description { get; set; }
        //https://www.census.gov/geo/reference/ansi_statetables.html
        [MaxLength(2)][Required]
        public string FIPSCode { get; set; }

        public List<CatagoryCoefficient> CatagoryCoefficients { get;set; }
        public List<RegionManager> RegionManagers { get; set; }
        public List<Source> Sources { get; set; }
    }
}
