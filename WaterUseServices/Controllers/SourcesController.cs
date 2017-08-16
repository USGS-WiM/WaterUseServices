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
    public class SourcesController : WUControllerBase
    {
        public SourcesController(IWaterUseAgent sa):base(sa)
        {}
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
                    //only return sources user has access to.
                    Int32 ID = LoggedInUser().ID;
                    if(ID < 1) return new UnauthorizedResult();
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
                                                    StationID = s.StationID,
                                                    Location = s.Location
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
                if (id < 0) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest)); // This returns HTTP 400
               

                var ObjectRequested = agent.Select<Source>().Include(s => s.Region)
                                            .ThenInclude(s => s.RegionManagers).FirstOrDefault(x => x.ID == id);

                if(ObjectRequested == null) return new BadRequestObjectResult(new Error(errorEnum.e_notFound)); // This returns HTTP 400 

                if (ObjectRequested.Region.RegionManagers.All(i => i.ManagerID != LoggedInUser().ID) && !User.IsInRole("Administrator"))
                    return new UnauthorizedResult();

                return Ok(new Source()
                {
                    ID = ObjectRequested.ID,
                    CatagoryTypeID = ObjectRequested.CatagoryTypeID,
                    FacilityCode = ObjectRequested.FacilityCode,
                    FacilityName = ObjectRequested.FacilityName,
                    Name = ObjectRequested.Name,
                    RegionID = ObjectRequested.RegionID,
                    SourceTypeID = ObjectRequested.SourceTypeID,
                    StationID = ObjectRequested.StationID,
                    Location = ObjectRequested.Location
                });
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
                if (!User.IsInRole("Administrator") && !agent.Select<Manager>().Include(m=>m.RegionManagers)
                                                            .Where(m => m.ID == LoggedInUser().ID)
                                                            .Any(m => m.RegionManagers.Select(rm=>rm.RegionID).Contains(entity.RegionID)))
                    return new UnauthorizedResult();


                if (!isValid(entity)) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest)); // This returns HTTP 400
                return Ok(await agent.Add<Source>(entity));
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
                if (!User.IsInRole("Administrator") && !entities.Select(x => x.RegionID).All(rid =>
                                        agent.Select<Manager>().Include(m => m.RegionManagers).Where(rm => rm.ID == LoggedInUser().ID)
                                                            .SelectMany(i => i.RegionManagers.Select(rm => rm.RegionID)).Contains(rid)))
                    return new UnauthorizedResult();
                bool isOK = true;

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
                var ObjectToBeUpdated = agent.Select<Source>().Include(s => s.Region)
                                            .ThenInclude(s => s.RegionManagers).FirstOrDefault(x => x.ID == id);

                if (ObjectToBeUpdated.Region.RegionManagers.All(i => i.ManagerID != LoggedInUser().ID) && !User.IsInRole("Administrator"))
                    return new UnauthorizedResult();

                if (!User.IsInRole("Administrator"))
                    //only admin can change region
                   entity.RegionID = ObjectToBeUpdated.RegionID;           

                if (id < 0 || !isValid(entity)) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest)); // This returns HTTP 400
                return Ok(await agent.Update<Source>(id, entity));
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

                var ObjectToBeDeleted = agent.Select<Source>().Include(s => s.Region)
                                            .ThenInclude(s => s.RegionManagers).FirstOrDefault(x => x.ID == id);

                if (ObjectToBeDeleted.Region.RegionManagers.All(i => i.ManagerID != LoggedInUser().ID) && !User.IsInRole("Administrator"))
                    return new UnauthorizedResult();


                var entity = await agent.Find<Source>(id);
                if (entity == null) return new NotFoundObjectResult(new Error());
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
