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

namespace WaterUseServices.Controllers
{
    [Route("wateruse/[controller]")]
    public class SourceTypesController : Controller
    {
        private IWaterUseAgent agent;

        public SourceTypesController(IWaterUseAgent sa)
        {
            this.agent = sa;
        }
        #region METHODS
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(agent.Select<SourceType>());
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id < 0) return new BadRequestResult(); // This returns HTTP 404

                return Ok(agent.Find<SourceType>(id));
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpPost][Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Post([FromBody]SourceType entity)
        {
            try
            {
                if (!isValid(entity)) return new BadRequestResult();
                return Ok(agent.Add<SourceType>(entity));
            }
            catch (Exception)
            {
                throw;
            }
        }
     
        [HttpPut("{id}")][Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Put(int id, [FromBody]SourceType entity)
        {
            try
            {
                if (id < 0 || !isValid(entity)) return new BadRequestResult(); // This returns HTTP 404
                return Ok(agent.Update<SourceType>(id, entity));
            }
            catch (Exception)
            {

                throw;
            }

        }
        
        [HttpDelete("{id}")][Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                if (id < 1) return new BadRequestResult();
                var entity = agent.Find<SourceType>(id);
                if (entity == null) return new BadRequestResult();
                agent.Delete<SourceType>(entity);

                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region HELPER METHODS
        private Boolean isValid(SourceType item)
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
