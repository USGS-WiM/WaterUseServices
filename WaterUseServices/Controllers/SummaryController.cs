﻿//------------------------------------------------------------------------------
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
        [HttpPost][HttpGet]
        public async Task<IActionResult> Get([FromQuery] Int32 startyear, [FromQuery]Int32? endyear = null, [FromBody] object basin = null, [FromQuery]string sources = "")
        {
            try
            {
                if (startyear < 1900 || (basin == null && string.IsNullOrEmpty(sources))) return new BadRequestResult(); //return HTTP 404

                if (!string.IsNullOrEmpty(sources))
                {
                    if (!User.Identity.IsAuthenticated) return new UnauthorizedResult();// return HTTP 401
                    return Ok(agent.GetWateruse(parse(sources), startyear, endyear));
                }//end if

                if (basin == null) return new BadRequestResult(); //return HTTP 404
                return Ok(agent.GetWateruse(basin, startyear, endyear));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost][HttpGet]
        [Route("BySource")]
        public async Task<IActionResult> BySource([FromQuery] Int32 startyear, [FromQuery]Int32? endyear = null, [FromBody] object basin = null, [FromQuery]string sources = "")
        {
            try
            {
                if (startyear < 1900 || (basin == null && string.IsNullOrEmpty(sources))) return new BadRequestResult(); //return HTTP 404

                if (!string.IsNullOrEmpty(sources))
                {
                    if (!User.Identity.IsAuthenticated) return new UnauthorizedResult(); //return HTTP 404                   
                    return Ok(agent.GetWaterusebySource(parse(sources), startyear, endyear));
                }//end if

                if (basin == null) return new BadRequestResult(); //return HTTP 401
                return Ok(agent.GetWaterusebySource(basin, startyear, endyear));
            }
            catch (Exception)
            {
                throw;
            }
        }
                
        #endregion
        #region HELPER METHODS
       
        #endregion
    }
}
