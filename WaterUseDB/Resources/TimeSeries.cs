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

using System;

namespace WaterUseDB.Resources
{
    public class TimeSeries
    {
        public int ID { get; set; }
        public int SourceID { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public int UnitTypeID { get; set; }

        public Source Source { get; set; }
        public UnitType UnitType { get; set; }
    }
}
