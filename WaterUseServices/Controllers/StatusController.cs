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

namespace WaterUseServices.Controllers
{
    [Route("wateruse/[controller]")]
    public class StatusController : Controller
    {
        private WaterUseServices.Data.WaterUseServiceAgent agent;

        public StatusController(WaterUseServices.Data.WaterUseServiceAgent sa) {
            this.agent = sa;
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(agent.Select<StatusType>());        
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if(id<0) return new BadRequestResult(); // This returns HTTP 404

            return Ok(agent.Find<StatusType>(id));
        }
       
        [HttpPost][Authorize(Policy = "AdminOnly")]
        public IActionResult Post([FromBody]StatusType entity)
        {
            try
            {
                return Ok(agent.Add<StatusType>(entity));
            }
            catch (Exception)
            {
                throw;
            }            
        }
        
        [HttpPut("{id}")][Authorize(Policy = "AdminOnly")]
        public IActionResult Put(int id, [FromBody]StatusType entity)
        {
            return Ok(agent.Update<StatusType>(id,entity));
        }
        
        [HttpDelete("{id}")][Authorize(Policy = "AdminOnly")]
        public void Delete(int id)
        {
            var role = agent.Find<StatusType>(id);
            if(role != null)
                agent.Delete<StatusType>(role);
        }
    }
}
