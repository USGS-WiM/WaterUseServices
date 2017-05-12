//------------------------------------------------------------------------------
//----- ServiceAgent -------------------------------------------------------
//------------------------------------------------------------------------------

//-------1---------2---------3---------4---------5---------6---------7---------8
//       01234567890123456789012345678901234567890123456789012345678901234567890
//-------+---------+---------+---------+---------+---------+---------+---------+

// copyright:   2017 WiM - USGS

//    authors:  Jeremy K. Newson USGS Web Informatics and Mapping
//              
//  
//   purpose:   The service agent is responsible for initiating the service call, 
//              capturing the data that's returned and forwarding the data back to 
//              the requestor.
//
//discussion:   delegated hunting and gathering responsibilities.   
//
// 
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WaterUseDB;
using WaterUseDB.Resources;
using WiM.Utilities;

namespace WaterUseServices.Data
{
    public class WaterUseServiceAgent:DBAgentBase
    {
        public WaterUseServiceAgent(WaterUseDBContext context):base(context) {
            
            //optimize query for disconnected databases.
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        #region Roles
        public IQueryable<Role> GetRoles() {
            try
            {
                return this.Select<Role>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Manager
        public Manager GetManagerByUsername(string username) {
            try
            {
                return this.Select<Manager>().Include(p=>p.Role).FirstOrDefault(u=>string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception)
            {
                throw;
            }


        }
        #endregion
    }
}
