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
using WaterUseServices.Resources;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WaterUseAgent.Extensions;

namespace WaterUseServices.Controllers
{
    [Route("[controller]")]
    public class TimeSeriesController : WUControllerBase
    {
        public TimeSeriesController(IWaterUseAgent sa) : base(sa)
        {}
        #region METHODS
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id < 0) return new BadRequestResult(); // This returns HTTP 404

                return Ok(await agent.Find<TimeSeries>(id));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }
        
        [HttpPost][Authorize(Policy = "CanModify")]
        public async Task<IActionResult> Post([FromBody]TimeSeries entity)
        {
            try
            {
                if (! isValid(entity)) return new BadRequestResult(); // This returns HTTP 404
                return Ok(await agent.Add<TimeSeries>(entity));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        [HttpPost("/Regions/{regionID}/[controller]/Batch")]
        [Authorize(Policy = "CanModify")]
        public async Task<IActionResult> Batch(int regionID, [FromBody]List<FCTimeSeries> entities)
        {
            Dictionary<int, Error> msgs = new Dictionary<int, Error>();
            try
            {
                
                var entityFClist = entities.AddFCFIPCode(agent.GetRegionByIDOrShortName(regionID.ToString()).FIPSCode).Select(e => e.FacilityCode).Distinct().ToList();
                var regionSources = agent.Select<Source>().Where(s => s.RegionID == regionID && entityFClist.Contains(s.FacilityCode))
                                        .Include("Region.RegionManagers").ToList();


                if (!isValid(entities, regionSources, ref msgs))
                    return new BadRequestObjectResult(msgs);

                List<TimeSeries> items = entities.Select(ts => new TimeSeries()
                {
                    Date = new DateTime(ts.Date.Year, ts.Date.Month, 1),
                    UnitTypeID = ts.UnitTypeID,
                    Value = ts.Value,
                    SourceID = regionSources.First(s=>String.Equals(s.FacilityCode,ts.FacilityCode,StringComparison.OrdinalIgnoreCase)).ID
                }).ToList();

                if (!isValid(items)) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest, "One or more items are invalid"));

                return Ok(await agent.Add<TimeSeries>(items));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        [HttpPut("{id}")][Authorize(Policy = "CanModify")]
        public async Task<IActionResult> Put(int id, [FromBody]TimeSeries entity)
        {
            try
            {
                if (id <0 || !isValid(entity)) return new BadRequestResult(); // This returns HTTP 404
                return Ok(await agent.Update<TimeSeries>(id, entity));
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
                if (id < 1) return new BadRequestResult();
                var entity = await agent.Find<TimeSeries>(id);
                if (entity == null) return new NotFoundResult();

                await agent.Delete<TimeSeries>(entity);

                return Ok();

            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }
        #endregion
        #region HELPER METHODS
        protected bool isValid(List<FCTimeSeries> items, List<Source> sources, ref Dictionary<int, Error> msgs)
        {
            
            var loggedInManager = LoggedInUser();
            bool isOK = true;

            for (int i = 0; i < items.Count; i++)
            {
                var ts = items[i];
                if (!base.isValid(ts))
                {
                    msgs.Add(i, new Error(errorEnum.e_badRequest));
                    isOK = false;
                    continue;
                }//end if

                var source = sources.FirstOrDefault(s => String.Equals(s.FacilityCode, ts.FacilityCode, StringComparison.OrdinalIgnoreCase));
                //ensure source is available
                if (source == null)
                {
                    msgs.Add(i, new Error(errorEnum.e_notFound,"No source exists with specified FacilityCode: " + ts.FacilityCode));
                    isOK = false;
                    continue;
                }//end if

                //check authentication
                if (!source.Region.RegionManagers.Any(rm => rm.ManagerID == loggedInManager.ID) && !User.IsInRole("Administrator"))
                {
                    msgs.Add(i, new Error(errorEnum.e_unauthorize,"User unauthorized for specified FacilityCode: " + ts.FacilityCode));
                    isOK = false;
                    continue;
                }//end if
            }//next i
            return isOK;
        }
        #endregion
    }
}
