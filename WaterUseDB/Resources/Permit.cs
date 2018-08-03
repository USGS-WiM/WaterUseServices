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

using System;
using System.ComponentModel.DataAnnotations;

namespace WaterUseDB.Resources
{
    public partial class Permit
    {
        public int ID { get; set; }
        [Required]
        public int SourceID { get; set; }
        [Required]
        public string PermitNO { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Double? WellCapacity { get; set; }
        public Double? IntakeCapacity { get; set; }
        public int? UnitTypeID { get; set; }
        public int? StatusTypeID { get; set; }
       
        public Source Source { get; set; }
        public UnitType UnitType { get; set; }
                
        public StatusType StatusType { get; set; }
    }
}
