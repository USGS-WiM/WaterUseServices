//------------------------------------------------------------------------------
//----- HttpController ---------------------------------------------------------
//------------------------------------------------------------------------------

//-------1---------2---------3---------4---------5---------6---------7---------8
//       01234567890123456789012345678901234567890123456789012345678901234567890
//-------+---------+---------+---------+---------+---------+---------+---------+

// copyright:   2017 WiM - USGS

//    authors:  Jeremy K. Newson USGS Web Informatics and Mapping
//              
//  
//   purpose:   Handles resources through the HTTP uniform interface.
//
//discussion:   Controllers are objects which handle all interaction with resources. 
//              
//
// 

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WaterUseDB.Resources;
using WaterUseAgent;
using System.Threading.Tasks;

namespace WaterUseServices.Controllers
{
    [Route("wateruse/[controller]")]
    public class SummaryController : NSSControllerBase
    {
        private IWaterUseAgent agent;

        public SummaryController(IWaterUseAgent sa) {
            this.agent = sa;
        }

        #region METHOD
        [HttpGet][Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Get([FromQuery]string sources, [FromQuery] Int32 startyear, [FromQuery]Int32 endyear)
        {
            List<string> sourcelist = null; 
            try
            {
                sourcelist = parse(sources);
                if (string.IsNullOrEmpty(sources) || startyear <1900 || sourcelist.Count < 1) return new BadRequestResult(); // This returns HTTP 404

                return Ok(agent.GetWateruse(sourcelist, startyear, endyear));
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> Getbybasin([FromBody] string basin, [FromQuery] Int32 startyear, [FromQuery]Int32 endyear)
        {
            try
            {
                 if (string.IsNullOrEmpty(basin) || startyear < 1900) return new BadRequestResult(); // This returns HTTP 404

                return Ok(agent.GetWateruse(basin, startyear, endyear));
            }
            catch (Exception)
            {
                throw;
            }            
        }

        #endregion
        #region HELPER METHODS
        private Boolean isValid(UnitType item)
        {
            try
            {
                return string.IsNullOrEmpty(item.Name);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
