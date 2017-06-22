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
using WaterUseDB.Resources;
using WaterUseAgent;
using System.Threading.Tasks;

namespace WaterUseServices.Controllers
{
    [Route("wateruse/[controller]")]
    public class SourcesController : Controller
    {
        private IWaterUseAgent agent;

        public SourcesController(IWaterUseAgent sa)
        {
            this.agent = sa;
        }
        #region METHODS
        [HttpGet]
        [Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(agent.Select<Source>());
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        
        [HttpGet("{id}")]
        [Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id < 0) return new BadRequestResult(); // This returns HTTP 404

                return Ok(agent.Find<Source>(id));
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpPost][Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Post([FromBody]Source entity)
        {
            try
            {
                if (!isValid(entity)) return new BadRequestResult();
                return Ok(agent.Add<Source>(entity));
            }
            catch (Exception)
            {
                throw;
            }
        }
     
        [HttpPut("{id}")][Authorize(Policy = "CanModify")]
        public async Task<IActionResult> Put(int id, [FromBody]Source entity)
        {
            try
            {
                if (id < 0 || !isValid(entity)) return new BadRequestResult(); // This returns HTTP 404
                return Ok(agent.Update<Source>(id, entity));
            }
            catch (Exception)
            {

                throw;
            }

        }
        
        [HttpDelete("{id}")][Authorize(Policy = "Restricted")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                if (id < 1) return new BadRequestResult();
                var entity = agent.Find<Source>(id);
                if (entity == null) return new BadRequestResult();
                agent.Delete<Source>(entity);

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region HELPER METHODS
        private Boolean isValid(Source item)
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
