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
using System.Threading.Tasks;

namespace WaterUseAgent
{
    public interface IWaterUseAgent
    {
        Boolean IncludePermittedWithdrawals {set; }
        IQueryable<T> Select<T>() where T : class, new();
        Task<T> Find<T>(Int32 pk) where T : class, new();
        Task<T> Add<T>(T item) where T : class, new();
        Task<IEnumerable<T>> Add<T>(List<T> items) where T : class, new();
        Task<T> Update<T>(Int32 pkId, T item) where T : class, new();
        Task Delete<T>(T item) where T : class, new();
        IQueryable<Role> GetRoles();
        Manager GetManagerByUsername(string username);
        Wateruse GetWateruse(List<string> sources, Int32 startyear, Int32? endyear);
        Wateruse GetWateruse(object basin, Int32 startyear, Int32? endyear);
        IDictionary<string, Wateruse> GetWaterusebySource(List<string> sources, Int32 startyear, Int32? endyear);
        IDictionary<string,Wateruse> GetWaterusebySource(object basin, int startyear, int? endyear);
    }
    public class WaterUseServiceAgent : DBAgentBase, IWaterUseAgent
    {
        public bool IncludePermittedWithdrawals { private get; set; }

        public WaterUseServiceAgent(WaterUseDBContext context) : base(context) {

            //optimize query for disconnected databases.
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        #region Universal
        public new IQueryable<T> Select<T>() where T : class, new()
        {
            return base.Select<T>();
        }
        public new Task<T> Find<T>(Int32 pk) where T : class, new()
        {
            return base.Find<T>(pk);
        }
        public new Task<T> Add<T>(T item) where T : class, new()
        {
            return base.Add<T>(item);
        }
        public new Task<IEnumerable<T>> Add<T>(List<T> items) where T : class, new()
        {
            return base.Add<T>(items);
        }
        public new Task<T> Update<T>(Int32 pkId, T item) where T : class, new()
        {
            return base.Update<T>(pkId, item);
        }
        public new Task Delete<T>(T item) where T : class, new()
        {
            return base.Delete<T>(item);
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
        #region Summary
        public Wateruse GetWateruse(List<string> sources, Int32 startyear, Int32? endyear)
        {
            IQueryable<Source> equery = null;
            int parsed = 0;
            try
            {
                if (sources == null && sources.Count() <= 0) return null;
                IEnumerable<int> ids = sources.Where(x => int.TryParse(x, out parsed)).Select(x => parsed);

                equery = FromSQL<Source>(String.Format(getSQLStatement(sqlTypes.e_source),
                                                        ids.Count() < 1 ? "-999" : String.Join("','", ids),
                                                        String.Join("','", sources)));

                var result = getAggregatedWaterUse(equery, startyear, endyear);
                return (result);

            }
            catch (Exception ex)
            {
                sm(WiM.Resources.MessageType.error, "Error aggregating Wateruse " + ex.Message);
                return null;
            }
        }
        public Wateruse GetWateruse(object basin, Int32 startyear, Int32? endyear)
        {
            IQueryable<Source> equery = null;
            try
            {
                equery = FromSQL<Source>(String.Format(getSQLStatement(sqlTypes.e_sourcebygeojson), basin, 4326));

                var result = getAggregatedWaterUse(equery, startyear, endyear);
                return (result);


            }
            catch (Exception ex)
            {
                sm(WiM.Resources.MessageType.error, "Error aggregating Wateruse " + ex.Message);
                return null;
            }
        }
        public IDictionary<string, Wateruse> GetWaterusebySource(List<string> sources, Int32 startyear, Int32? endyear)
        {
            IQueryable<Source> equery = null;
            int parsed = 0;
            try
            {
                if (sources == null && sources.Count() <= 0) return null;
                IEnumerable<int> ids = sources.Where(x => int.TryParse(x, out parsed)).Select(x => parsed);

                equery = FromSQL<Source>(String.Format(getSQLStatement(sqlTypes.e_source),
                                                        ids.Count() < 1 ? "-999" : String.Join("','", ids),
                                                        String.Join("','", sources)));

                return getAggregatedWaterUseBySource(equery, startyear, endyear);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IDictionary<string, Wateruse> GetWaterusebySource(object basin, int startyear, int? endyear)
        {
            IQueryable<Source> equery = null;
            try
            {
                equery = FromSQL<Source>(String.Format(getSQLStatement(sqlTypes.e_sourcebygeojson), basin, 4326));

                return getAggregatedWaterUseBySource(equery, startyear, endyear);
            }
            catch (Exception ex)
            {
                sm(WiM.Resources.MessageType.error, "Error aggregating Wateruse " + ex.Message);
                return null;
            }
        }
        #endregion
        #region HELPER METHODS

        private Wateruse getAggregatedWaterUse(IQueryable<Source> sources, Int32 startyear, Int32? endyear)
        {
            List<Source> sourceList = null;
            try
            {
                if (IncludePermittedWithdrawals) sources = sources.Include(s => s.Permits).Include("TimeSeries.UnitType");
                sourceList = sources.Include(s => s.SourceType).Include("TimeSeries.UnitType").Include(s => s.CatagoryType).ToList();
                
                return new Wateruse()
                {                    
                    ProcessDate = DateTime.Now,
                    StartYear = startyear,
                    EndYear = endyear,
                    Return = getWaterUseSummary(sourceList.Where(s => s.SourceType.Code == "SW").ToList(), startyear, endyear),
                    Withdrawal = getWaterUseSummary(sourceList.Where(s => s.SourceType.Code == "GW").ToList(), startyear, endyear)
                };
            }   
            catch (Exception)
            {
                throw;
            }
        }      
        private IDictionary<string, Wateruse> getWateruseBySource(IList<Source> sources, int startyear, int? endyear)
        {
            Dictionary<string, Wateruse> result = new Dictionary<string, Wateruse>();

            foreach (var item in sources)
            { 
                result.Add(item.FacilityCode, getAggregatedWaterUse(sources.Where(i => i.Equals(item)).AsQueryable(), startyear, endyear));
            }//next item
            return result;
        }

        private IDictionary<string, Wateruse> getAggregatedWaterUseBySource(IQueryable<Source> sources, Int32 startyear, Int32? endyear)
        {
            List<Source> sourceList = null;
            Dictionary<string, Wateruse> result = new Dictionary<string, Wateruse>();
            try
            {
                sourceList = sources.Include(s => s.SourceType).Include("TimeSeries.UnitType").Include(s => s.CatagoryType).ToList();
                foreach (var item in sourceList)
                {
                    var wu = getWaterUseSummary(sourceList.Where(s => s.Equals(item)).ToList(), startyear, endyear);
                    result.Add(item.FacilityCode, new Wateruse()
                    {
                        ProcessDate = DateTime.Now,
                        StartYear = startyear,
                        EndYear = endyear,
                        Return = item.SourceType.Code == "SW" ? wu : null,
                        Withdrawal = item.SourceType.Code == "GW" ? wu : null,
                    });

                }//next item
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private WateruseSummary getWaterUseSummary(IList<Source> sources, Int32 startyear, Int32? endyear)
        {
            List<TimeSeries> tslist = null;
            Int32 yrspan = 1;
            List<Permit> pmtlist = null;
            SourceType sType = null;

            try
            {
                if (!endyear.HasValue) endyear = startyear;
                yrspan = (endyear.Value + 1) - startyear;

                tslist = sources.SelectMany(s => s.TimeSeries).Where(ts => ts.Date.Year >= startyear && ts.Date.Year <= endyear.Value).ToList();
                if (this.IncludePermittedWithdrawals)
                    pmtlist = sources.SelectMany(s => s.Permits).Where(p => p.StartDate.HasValue && p.StartDate.Value.Year >= startyear && 
                                                                                p.StartDate.Value.Year <= endyear.Value).ToList();

                if (tslist.Count < 1) return null;
                sType = tslist.First().Source.SourceType;

                return new WateruseSummary() {
                    Annual = new WateruseValue() {
                        Name = sType.Name,
                        Description = "Daily Annual Average " + sType.Description,
                        Value = tslist.Sum(ts => ts.Value * DateTime.DaysInMonth(ts.Date.Year, ts.Date.Month) / getDaysInYear(ts.Date.Year)),
                        Unit = tslist.First().UnitType
                    },
                    Monthly = tslist.GroupBy(ts => ts.Date.Month)
                    .ToDictionary(ky => ky.Key, mval => new MonthlySummary()
                    {
                        Month = new WateruseValue() {
                            Name = mval.First().Date.ToString("MMM", CultureInfo.InvariantCulture),
                            Description = mval.First().Date.ToString("MMM", CultureInfo.InvariantCulture) + " daily monthly average",
                            Unit = mval.First().UnitType,
                            Value = mval.Sum(ts => ts.Value / yrspan)
                        },
                        Code = mval.Any(i => i.Source.CatagoryTypeID.HasValue) ? mval.GroupBy(cd => cd.Source.CatagoryType.Code)
                        .ToDictionary(ky => ky.Key, cval => new WateruseValue()
                        {
                            Name = cval.First().Source.CatagoryType.Name,
                            Description = "Daily " + cval.First().Date.ToString("MMM", CultureInfo.InvariantCulture) + " average " + cval.First().Source.CatagoryType.Name,
                            Unit = cval.First().UnitType,
                            Value = cval.Sum(ts => ts.Value / yrspan)
                        }) : null
                    }),
                      Permitted = pmtlist !=null && pmtlist.Count >0 ? new PermittedSummary() {
                        Well = new WateruseValue() {
                            Name = "Permitted " + sType.Name,
                            Description = "Daily Annual Average Permitted" + sType.Description,
                            Value = pmtlist.Where(p=>p.WellCapacity.HasValue).Sum(p => p.WellCapacity.Value * DateTime.DaysInMonth(p.StartDate.Value.Year, p.StartDate.Value.Month) / getDaysInYear(p.StartDate.Value.Year)),
                            Unit = pmtlist.First().UnitType
                        },
                        Intake = new WateruseValue()
                        {
                            Name = "Permitted " + sType.Name,
                            Description = "Daily Annual Average Permitted" + sType.Description,
                            Value = pmtlist.Where(p => p.IntakeCapacity.HasValue).Sum(p => p.IntakeCapacity.Value * DateTime.DaysInMonth(p.StartDate.Value.Year, p.StartDate.Value.Month) / getDaysInYear(p.StartDate.Value.Year)),
                            Unit = pmtlist.First().UnitType
                        },
                      }:null                     
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

        private string getSQLStatement(sqlTypes type) {

            switch (type)
            {
                case sqlTypes.e_sourcebygeojson:
                    return @"select * from ""public"".""Sources""
                                where ST_Within(
                                    ""Location"",
                                    st_transform(
                                        st_setsrid(
                                            ST_GeomFromGeoJSON('{{{0}}}'),
                                        {1}),
                                    4269))";
                case sqlTypes.e_source:
                    return @"SELECT * FROM ""public"".""Sources"" 
                                WHERE ""ID"" IN ('{0}') OR 
                                LOWER(""FacilityCode"") IN ('{1}')";


                default:
                    throw new Exception("No sql for table " + type);
            }
        }
        #endregion
        #region Enumerations
        private enum sqlTypes
        {
            e_sourcebygeojson,
            e_source
        }
        #endregion
    }
}
