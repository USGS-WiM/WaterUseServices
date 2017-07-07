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

namespace WaterUseServices.Controllers
{
    [Route("[controller]")]
    public class PermitController : NSSControllerBase
    {
        private IWaterUseAgent agent;

        public PermitController(IWaterUseAgent sa) {
            this.agent = sa;
        }
        #region METHODS
        [HttpGet][Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(agent.Select<Permit>());   
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }                 
        }
        
        [HttpGet("{id}")][Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if(id<0) return new BadRequestResult(); // This returns HTTP 404

                return Ok(await agent.Find<Permit>(id));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }            
        }
      
        [HttpPost][Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Post([FromBody]Permit entity)
        {
            try
            {
                if (!isValid(entity)) return new BadRequestResult();

                return Ok(await agent.Add<Permit>(entity));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }            
        }

        [HttpPost][Authorize(Policy = "Restricted")]
        [Route("Batch")]
        public async Task<IActionResult> Batch([FromBody]List<Permit> entities)
        {
            try
            {
                if (!isValid(entities)) return new BadRequestObjectResult("Object is invalid");

                return Ok(await agent.Add<Permit>(entities));
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
                var entityToDelete = await agent.Find<Permit>(id);
                if (entityToDelete == null) return new BadRequestResult();

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
