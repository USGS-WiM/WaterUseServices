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
using WaterUseAgent.Extensions;

namespace WaterUseServices.Controllers
{
    [Route("[controller]")]
    public class SourcesController : WUControllerBase
    {
        public SourcesController(IWaterUseAgent sa):base(sa)
        {}
        #region METHODS
        [HttpGet]
        [Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Get([FromQuery] int? RegionID = null)
        {
            IQueryable<Region> regionquery = null;
            try
            {                
                if (!User.IsInRole("Administrator"))
                {
                    //only return sources user has access to.
                    Int32 ID = LoggedInUser().ID;
                    if(ID < 1) return new UnauthorizedResult();
                    regionquery = agent.GetManagedRegion(ID);
                }//end if
                else
                {
                    regionquery = agent.GetRegions();
                }

                if (RegionID.HasValue)
                {
                    regionquery = regionquery.Where(s => s.ID == RegionID);
                    if (regionquery.Count() < 1) return new BadRequestObjectResult(new Error(errorEnum.e_unauthorize, "User unauthorized, or invalid region requested"));
                }

                return Ok(agent.GetSources(regionquery));
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
            IQueryable<Region> regionquery = null;
            try
            {
                if (id < 0) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest)); // This returns HTTP 400

                if (!User.IsInRole("Administrator"))
                {
                    //only return sources user has access to.
                    Int32 ID = LoggedInUser().ID;
                    if (ID < 1) return new UnauthorizedResult();
                    regionquery = agent.GetManagedRegion(ID);
                }//end if
                else
                {
                    regionquery = agent.GetRegions();
                }
                
                var ObjectRequested = agent.GetSources(regionquery).FirstOrDefault(i=>i.ID == id);

                if(ObjectRequested == null) return new BadRequestObjectResult(new Error(errorEnum.e_notFound)); // This returns HTTP 400 

                return Ok(ObjectRequested);
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }       

        [HttpPost][Authorize(Policy = "CanModify")]
        public async Task<IActionResult> Post([FromBody]Source entity)
        {
            try
            {
                //verify user can submit to region
                if (!User.IsInRole("Administrator") && !agent.GetManagedRegion(LoggedInUser().ID).Any(r=>r.ID==entity.RegionID))
                return new UnauthorizedResult();

                if (!isValid(entity)) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest, "One or more of the properties are invalid.")); // This returns HTTP 400
               
                return Ok(agent.Add(entity));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        [HttpPost("/Regions/{region}/[controller]/Batch")]
        [HttpPost]
        [Authorize(Policy = "CanModify")]
        [Route("Batch")]
        public async Task<IActionResult> Batch([FromBody]List<Source> entities, string region = "")
        {
            Dictionary<int, Error> msgs = new Dictionary<int, Error>();
            try
            {
                if(entities == null || entities.Count < 1) return new BadRequestObjectResult("Request must have a valid body");

                if (!String.IsNullOrEmpty(region))
                {
                    var item = agent.GetRegionByIDOrShortName(region);
                    if(item == null) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest, "No region exists with supplied ID."));
                    entities.ForEach(e => e.RegionID = item.ID);
                }//end if

                //verify user can submit to region
                if (!User.IsInRole("Administrator") && !entities.All(entity=> agent.GetManagedRegion(LoggedInUser().ID).Any(r => r.ID == entity.RegionID)))
                    return new UnauthorizedResult();
                bool isOK = true;

                var fcipcode = agent.GetRegions().ToDictionary(k => k.ID, k => k.FIPSCode);

                for (int i = 0; i < entities.Count; i++)
                {
                    var s = entities[i];
                    
                    if (!isValid(s))
                    {
                        msgs.Add(i, new Error(errorEnum.e_badRequest));
                        isOK = false;
                        continue;
                    }//end if
                }//next i


                if (!isOK) return new BadRequestObjectResult(msgs);
               
                return Ok(agent.Add(entities));
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

                if (!User.IsInRole("Administrator") && !agent.GetManagedRegion(LoggedInUser().ID).Any(r => r.ID == entity.RegionID))
                    return new UnauthorizedResult();

                var ObjectToBeUpdated = agent.GetSource(id,false);

                if (!User.IsInRole("Administrator"))
                {
                    //only admin can change region
                    entity.RegionID = ObjectToBeUpdated.RegionID;
                    
                }

                if (id < 0 || !isValid(entity)) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest)); // This returns HTTP 400

                return Ok(agent.Update(id, entity));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }
        
        [HttpDelete("{id}")][Authorize(Policy = "CanModify")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                if (id < 1) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest, "invalid id"));

                var ObjectToBeDeleted = agent.GetSource(id,false);
                if (ObjectToBeDeleted == null) return new NotFoundResult();

                if (!User.IsInRole("Administrator") && !agent.GetManagedRegion(LoggedInUser().ID).Any(r => r.ID == ObjectToBeDeleted.RegionID))
                    return new UnauthorizedResult();

                await agent.Delete<Source>(id);

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
