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
    [Route("[controller]")]
    public class SummaryController : NSSControllerBase
    {
        private IWaterUseAgent agent;

        public SummaryController(IWaterUseAgent sa) {
            this.agent = sa;
        }

        #region METHOD
        [HttpPost][HttpGet]
        public async Task<IActionResult> Get([FromQuery] Int32 year, [FromQuery]Int32? endyear = null, [FromBody] object basin = null, 
                                                [FromQuery]string sources = "",[FromQuery]bool includePermits = false, 
                                                [FromQuery]double? domesticGW = null, [FromQuery]double? domesticSW = null)
        {
            try
            {
                if (year < 1950 || (basin == null && string.IsNullOrEmpty(sources))) return new BadRequestResult(); //return HTTP 404

                if (includePermits) agent.IncludePermittedWithdrawals = includePermits;
                if (domesticGW.HasValue || domesticSW.HasValue)
                    agent.DomesticUse = new WaterUseAgent.Resources.Domestic() { GroundWater = domesticGW, SurfaceWater = domesticSW };

                if (!string.IsNullOrEmpty(sources))
                {
                    if (!User.Identity.IsAuthenticated) return new UnauthorizedResult();// return HTTP 401
                    return Ok(agent.GetWateruse(parse(sources), year, endyear));
                }//end if

                if (basin == null) return new BadRequestResult(); //return HTTP 404
                return Ok(agent.GetWateruse(basin, year, endyear));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        [HttpPost][HttpGet]
        [Authorize(Policy = "Restricted")]
        [Route("BySource")]
        public async Task<IActionResult> BySource([FromQuery] Int32 year, [FromQuery]Int32? endyear = null, [FromBody] object basin = null,
                                                    [FromQuery]string sources = "",[FromQuery]bool includePermits = false,
                                                    [FromQuery]double? domesticGW = null, [FromQuery]double? domesticSW = null)
        {
            try
            {
                if (year < 1950 || (basin == null && string.IsNullOrEmpty(sources))) return new BadRequestResult(); //return HTTP 404

                if (includePermits)
                    agent.IncludePermittedWithdrawals = includePermits;
                if (domesticGW.HasValue || domesticSW.HasValue)
                    agent.DomesticUse = new WaterUseAgent.Resources.Domestic() { GroundWater = domesticGW, SurfaceWater = domesticSW }; 

                if (!string.IsNullOrEmpty(sources))
                {
                    if (!User.Identity.IsAuthenticated) return new UnauthorizedResult(); //return HTTP 404                   
                    return Ok(agent.GetWaterusebySource(parse(sources), year, endyear));
                }//end if

                if (basin == null) return new BadRequestResult(); //return HTTP 401
                return Ok(agent.GetWaterusebySource(basin, year, endyear));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }
                
        #endregion
        #region HELPER METHODS
        #endregion
    }
}
