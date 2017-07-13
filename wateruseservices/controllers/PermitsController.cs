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
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WaterUseServices.Controllers
{
    [Route("[controller]")]
    public class PermitsController : WUControllerBase
    {
        public PermitsController(IWaterUseAgent sa) : base(sa)
        {}
        #region METHODS
        [HttpGet("/Sources/{sourceID}/[controller]")]
        [Authorize(Policy = "Restricted")]
        public async Task<IActionResult> GetSourcePermit(int sourceID)
        {
            try
            {
                if (sourceID < 1) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest)); // This returns HTTP 400

                var ObjectRequested = agent.Select<Source>().Include(s => s.Region)
                                            .ThenInclude(s => s.RegionManagers).Include(s => s.Permits).FirstOrDefault(x => x.ID == sourceID);

                if (ObjectRequested == null) return new BadRequestObjectResult(new Error(errorEnum.e_notFound)); // This returns HTTP 400 

                if (ObjectRequested.Region.RegionManagers.All(i => i.ManagerID != LoggedInUser().ID) && !User.IsInRole("Administrator"))
                    return new UnauthorizedResult();


                return Ok(ObjectRequested.Permits.Select(p=>new Permit(){ EndDate= p.EndDate,
                                                                        ID = p.ID,
                                                                        IntakeCapacity =p.IntakeCapacity,
                                                                        PermitNO = p.PermitNO,
                                                                        SourceID = p.SourceID,
                                                                        StartDate = p.StartDate,
                                                                        StatusTypeID = p.StatusTypeID,
                                                                        UnitTypeID =p.UnitTypeID,
                                                                        WellCapacity = p.WellCapacity}));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        [HttpPost("/Sources/{sourceID}/[controller]")]
        [Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Post(int sourceID, [FromBody]Permit entity)
        {
            try
            {
                if (sourceID < 1) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest)); // This returns HTTP 400

                var ObjectSource = agent.Select<Source>().Include(s => s.Region)
                                            .ThenInclude(s => s.RegionManagers).FirstOrDefault(x => x.ID == sourceID);

                if (ObjectSource == null) return new BadRequestObjectResult(new Error(errorEnum.e_notFound,"Source not found.")); // This returns HTTP 400 

                if (ObjectSource.Region.RegionManagers.All(i => i.ManagerID != LoggedInUser().ID) && !User.IsInRole("Administrator"))
                    return new UnauthorizedResult();

                entity.SourceID = ObjectSource.ID;

                if (!isValid(entity)) return new BadRequestObjectResult(new Error(errorEnum.e_notFound));

                return Ok(await agent.Add<Permit>(entity));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }            
        }

        [HttpPut("{id}")][Authorize(Policy = "CanModify")]
        public async Task<IActionResult> Put(int id, [FromBody]Permit entity)
        {
            try
            {
                var ObjectToBeUpdated = agent.Select<Permit>().Include(p=>p.Source.Region.RegionManagers)
                                            .FirstOrDefault(x => x.ID == id);

                if (ObjectToBeUpdated.Source.Region.RegionManagers.All(i => i.ManagerID != LoggedInUser().ID) && !User.IsInRole("Administrator"))
                    return new UnauthorizedResult();

                if (!isValid(entity) || id < 1) return new BadRequestResult();

                return Ok(await agent.Update<Permit>(id,entity));
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

                var entityToDelete = agent.Select<Permit>().Include(p => p.Source.Region.RegionManagers)
                                            .FirstOrDefault(x => x.ID == id);

                if (entityToDelete == null) return new BadRequestResult();

                if (entityToDelete.Source.Region.RegionManagers.All(i => i.ManagerID != LoggedInUser().ID) && !User.IsInRole("Administrator"))
                    return new UnauthorizedResult();

                

                await agent.Delete<Permit>(entityToDelete);
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
