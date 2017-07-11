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
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WaterUseServices.Controllers
{
    [Route("[controller]")]
    public class SourcesController : NSSControllerBase
    {
        private IWaterUseAgent agent;

        public SourcesController(IWaterUseAgent sa)
        {
            this.agent = sa;
        }
        #region METHODS
        [HttpGet]
        [Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Get([FromQuery] int? RegionID = null)
        {
            IQueryable<Source> query = null;
            try
            {
                query = agent.Select<Source>();               

                if (!User.IsInRole("Administrator"))
                {
                    //get logged in users ID
                    Int32 ID = LoggedInUser().ID;
                    if(ID < 1) return new BadRequestResult(); // This returns HTTP 404
                    query = query.Include(s => s.Region).ThenInclude(s=>s.RegionManagers).Where(s=>s.Region.RegionManagers.Any(rm=>rm.ManagerID == ID));
                }//end if

                if (RegionID.HasValue)
                    query = query.Where(s => s.RegionID == RegionID);
                return Ok(query.Select(s => new Source()
                                                {
                                                    ID = s.ID,
                                                    CatagoryTypeID = s.CatagoryTypeID,
                                                    FacilityCode = s.FacilityCode,
                                                    FacilityName = s.FacilityName,
                                                    Name = s.Name,
                                                    RegionID = s.RegionID,
                                                    SourceTypeID = s.SourceTypeID,
                                                    StationID = s.StationID
                                                }));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id < 0) return new BadRequestResult(); // This returns HTTP 404

                var x = await agent.Find<Source>(id);

                return Ok(x);
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }
        

        [HttpPost][Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Post([FromBody]Source entity)
        {
            try
            {
                if (!isValid(entity)) return new BadRequestResult();
                return Ok(await agent.Add<Source>(entity));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        [HttpPost][Authorize(Policy = "AdminOnly")]
        [Route("Batch")]
        public async Task<IActionResult> Batch([FromBody]List<Source> entities)
        {
            try
            {
                if (!isValid(entities)) return new BadRequestObjectResult("Object is invalid");

                return Ok(await agent.Add<Source>(entities));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        [HttpPut("{id}")][Authorize(Policy = "CanModify")]
        public async Task<IActionResult> Put(int id, [FromBody]Source entity)
        {
            try
            {
                if (id < 0 || !isValid(entity)) return new BadRequestResult(); // This returns HTTP 404
                return Ok(await agent.Update<Source>(id, entity));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }

        }
        
        [HttpDelete("{id}")][Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                if (id < 1) return new BadRequestResult();
                var entity = await agent.Find<Source>(id);
                if (entity == null) return new BadRequestResult();
                await agent.Delete<Source>(entity);

                return Ok();
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
