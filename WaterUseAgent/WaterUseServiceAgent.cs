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

using System;
using System.Collections.Generic;
using System.Linq;
using WaterUseDB;
using WaterUseDB.Resources;
using WiM.Utilities;
using Microsoft.EntityFrameworkCore;
using WaterUseAgent.Resources;
using System.Globalization;

namespace WaterUseAgent
{
    public interface IWaterUseAgent
    {
        IQueryable<T> Select<T>() where T : class, new();
        T Find<T>(Int32 pk) where T : class, new();
        T Add<T>(T item) where T : class, new();
        T Update<T>(Int32 pkId, T item) where T : class, new();
        void Delete<T>(T item) where T : class, new();
        IQueryable<Role> GetRoles();
        Manager GetManagerByUsername(string username);
        Wateruse GetWateruse(List<string> sources, Int32 startyear, Int32? endyear);
        Wateruse GetWateruse(string basin, Int32 startyear, Int32? endyear);



    }
    public class WaterUseServiceAgent : DBAgentBase, IWaterUseAgent
    {
        public WaterUseServiceAgent(WaterUseDBContext context) : base(context) {

            //optimize query for disconnected databases.
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        #region Universal
        public new IQueryable<T> Select<T>() where T : class, new()
        {
            return base.Select<T>();
        }
        public new T Find<T>(Int32 pk) where T : class, new()
        {
            return base.Find<T>(pk);
        }
        public new T Add<T>(T item) where T : class, new()
        {
            return base.Add<T>(item);
        }
        public new T Update<T>(Int32 pkId, T item) where T : class, new()
        {
            return base.Update<T>(pkId, item);
        }
        public new void Delete<T>(T item) where T : class, new()
        {
            base.Delete<T>(item);
        }
        public Wateruse GetWateruse(List<string> sources, Int32 startyear, Int32? endyear)
        {
            IQueryable<Source> equery = null;
            try
            {
                if (sources == null && sources.Count() <= 0) return null;

                equery = equery.Where(s => sources.Contains(s.ID.ToString().Trim())
                                            || sources.Contains(s.FacilityName.ToLower().Trim()));

                equery.Include("TimeSeries.UnitType").Include(s=>s.SourceType).Include(s=>s.CatagoryType);

                return (getAggregatedWaterUse(equery, startyear, endyear));
               
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Wateruse GetWateruse(string basin, Int32 startyear, Int32? endyear)
        {
            try
            {
                //POSTGIS method that returns list of Sources
                List<string> sources = null;
                return GetWateruse(sources, startyear, endyear);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
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

        #region HELPER METHODS
        private Wateruse getAggregatedWaterUse(IEnumerable<Source> list, Int32 startyear, Int32? endyear)
        {
            try
            {
                return new Wateruse()
                {
                    ProcessDate = DateTime.Now,
                    StartYear = startyear,
                    EndYear = endyear,
                    Return = getWaterUseSummary(list.Where(s => s.CatagoryType.Code == "SW"), startyear, endyear),
                    Withdrawal = getWaterUseSummary(list.Where(s => s.CatagoryType.Code == "GW"), startyear, endyear)
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private WateruseSummary getWaterUseSummary(IEnumerable<Source> sources, Int32 startyear, Int32? endyear)
        {
            IQueryable<TimeSeries> query = null;
            Int32 yrspan = 1;
            try
            {
                if (endyear.HasValue) yrspan = (endyear.Value + 1) - startyear;


                query = sources.SelectMany(s => s.TimeSeries).Where(ts => ts.Date.Year >= startyear && ts.Date.Year <= endyear.Value).AsQueryable();

                return new WateruseSummary() {
                    Annual = new WateruseValue() {
                        Description = "Daily Annual Average "+ query.First().Source.SourceType.Description,
                        Value = query.Sum(ts => ts.Value * DateTime.DaysInMonth(ts.Date.Year, ts.Date.Month) / getDaysInYear(ts.Date.Year)),
                        Unit = query.First().UnitType
                    },
                    Monthly = query.GroupBy(ts => ts.Date.Month)
                    .ToDictionary(ky => ky.Key, mval => new MonthlySummary()
                    {
                        Month = new WateruseValue() {
                            Description = mval.First().Date.Month.ToString("MMM", CultureInfo.InvariantCulture),
                            Unit = mval.First().UnitType,
                            Value = mval.Sum(ts => ts.Value/yrspan)
                        },
                        Code = mval.GroupBy(cd=>cd.Source.CatagoryType.Code)
                        .ToDictionary(ky=>ky.Key, cval=> new WateruseValue()
                        {
                            Description = "Daily " + cval.First().Date.Month.ToString("MMM", CultureInfo.InvariantCulture) +" average "+cval.First().Source.CatagoryType.Description,
                            Unit = cval.First().UnitType,
                            Value = cval.Sum(ts=>ts.Value/yrspan)
                        })
                    })
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private int getDaysInYear(Int32 year)
        {
            DateTime thisYear = new DateTime(year, 1, 1);
            DateTime nextYear = new DateTime(year+1, 1, 1);

            return (nextYear - thisYear).Days;
        }


        #endregion
    }
}
