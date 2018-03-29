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
using WaterUseDB.Resources;
using WaterUseAgent;
using System.Threading.Tasks;
using System.Linq;

namespace WaterUseServices.Controllers
{
    public class ConfigurationController : WUControllerBase
    {
        public ConfigurationController(IWaterUseAgent sa) : base(sa)
        { }

        #region METHOD
        [HttpGet("Regions/{region}/Config")]
        public async Task<IActionResult> Config(string region)
        {
            try
            {
                var item = agent.GetRegionByIDOrShortName(region);

                if (item == null) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest,"No region exists with supplied ID.")); // This returns HTTP 404

                return Ok(agent.RegionConfigureationAsync(item.ID));
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
