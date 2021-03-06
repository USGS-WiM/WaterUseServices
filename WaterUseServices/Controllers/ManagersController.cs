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
using System.Collections.Generic;
using WiM.Security;
using System.Text;
using System.Linq;

namespace WaterUseServices.Controllers
{
    [Route("[controller]")]
    public class ManagersController : WUControllerBase
    {
        public ManagersController(IWaterUseAgent sa) : base(sa)
        {}
        #region METHODS
        [HttpGet][Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(agent.Select<Manager>().Select(m=>new Manager() {
                                                             ID = m.ID,
                                                             Email = m.Email,
                                                             FirstName=m.FirstName,
                                                             LastName = m.LastName,
                                                             OtherInfo = m.OtherInfo,
                                                             PrimaryPhone = m.PrimaryPhone,
                                                             RoleID = m.RoleID,
                                                             SecondaryPhone = m.SecondaryPhone,
                                                             Username=m.Username
                                                            }));
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }

        }

        [HttpGet("/Login")]
        [Authorize(Policy = "Restricted")]
        public async Task<IActionResult> GetLoggedInUser()
        {
            try
            {
                return Ok(LoggedInUser());
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }

        }

        [HttpGet("{id}")][Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                if (id < 0) return new BadRequestResult(); // This returns HTTP 404

                var x = await agent.Find<Manager>(id);
                //remove info not relevant
                x.Salt = string.Empty;
                x.Password = string.Empty;

                return Ok(x);
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }
        
        [HttpPost][Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Post([FromBody]Manager entity)
        {
            try
            {
                if(string.IsNullOrEmpty(entity.FirstName)|| string.IsNullOrEmpty(entity.LastName) || 
                    string.IsNullOrEmpty(entity.Username)|| string.IsNullOrEmpty(entity.Email) ||
                    entity.RoleID <1) return new BadRequestObjectResult(new Error( errorEnum.e_badRequest, "You are missing one or more required parameter.")); // This returns HTTP 404

                if (string.IsNullOrEmpty(entity.Password))
                    entity.Password = generateDefaultPassword(entity);
                else
                    entity.Password = Encoding.UTF8.GetString(Convert.FromBase64String(entity.Password));

                entity.Salt = Cryptography.CreateSalt();
                entity.Password = Cryptography.GenerateSHA256Hash(entity.Password, entity.Salt);

                if (!isValid(entity)) return new BadRequestResult(); // This returns HTTP 404
                var x = await agent.Add<Manager>(entity);
                //remove info not relevant
                x.Salt = string.Empty;
                x.Password = string.Empty;

                return Ok(x);
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }

        [HttpPut("{id}")][Authorize(Policy = "CanModify")]
        public async Task<IActionResult> Put(int id, [FromBody]Manager entity)
        {
            Manager ObjectToBeUpdated = null;
            try
            {
                if (string.IsNullOrEmpty(entity.FirstName) || string.IsNullOrEmpty(entity.LastName) ||
                    string.IsNullOrEmpty(entity.Username) || string.IsNullOrEmpty(entity.Email) ||
                    entity.RoleID < 1) return new BadRequestObjectResult(new Error(errorEnum.e_badRequest)); // This returns HTTP 404

                //fetch object, assuming it exists
                ObjectToBeUpdated = await agent.Find<Manager>(id);
                if (ObjectToBeUpdated == null) return new NotFoundObjectResult(entity);

                if (!User.IsInRole("Administrator")|| LoggedInUser().ID !=id)
                    return new UnauthorizedResult();// return HTTP 401

                ObjectToBeUpdated.FirstName = entity.FirstName;
                ObjectToBeUpdated.LastName = entity.LastName;
                ObjectToBeUpdated.OtherInfo = entity.OtherInfo;
                ObjectToBeUpdated.PrimaryPhone = entity.PrimaryPhone;
                ObjectToBeUpdated.SecondaryPhone = entity.SecondaryPhone;
                ObjectToBeUpdated.Username = entity.Username;
                ObjectToBeUpdated.Email = entity.Email;

                //admin can only change role
                if(User.IsInRole("Administrator"))
                    ObjectToBeUpdated.RoleID = entity.RoleID;

                //change password if needed
                if (!string.IsNullOrEmpty(entity.Password) && !Cryptography
                            .VerifyPassword(Encoding.UTF8.GetString(Convert.FromBase64String(entity.Password)),
                                                                    ObjectToBeUpdated.Salt, ObjectToBeUpdated.Password))
                {
                    ObjectToBeUpdated.Salt = Cryptography.CreateSalt();
                    ObjectToBeUpdated.Password = Cryptography.GenerateSHA256Hash(Encoding.UTF8
                                .GetString(Convert.FromBase64String(entity.Password)), ObjectToBeUpdated.Salt);

                }//end if

                var x = await agent.Update<Manager>(id, entity);

                //remove info not relevant
                x.Salt = string.Empty;
                x.Password = string.Empty;

                return Ok(x);
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }           
        }
        
        [HttpDelete("{id}")][Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 1) return new BadRequestResult();
                var entity = await agent.Find<Manager>(id);
                if (entity == null) return new NotFoundResult();

                await agent.Delete<Manager>(entity);

                return Ok();

            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex);
            }
        }
        #endregion
        #region HELPER METHODS
        private string generateDefaultPassword(Manager entity)
        {
            //WUDefau1t+numbercharInlastname+first2letterFirstName
            string generatedPassword = "WUDefau1t" + entity.LastName.Length + entity.FirstName.Substring(0, 2);

            return generatedPassword;
        }
        #endregion
    }
}
