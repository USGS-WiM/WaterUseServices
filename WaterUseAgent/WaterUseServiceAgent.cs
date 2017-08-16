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
using WiM.Security.Authentication.Basic;

namespace WaterUseAgent
{
    public interface IWaterUseAgent
    {
        Boolean IncludePermittedWithdrawals {set; }
        void ComputeDomesticWateruse();
        IQueryable<T> Select<T>() where T : class, new();
        Task<T> Find<T>(Int32 pk) where T : class, new();
        Task<T> Add<T>(T item) where T : class, new();
        Task<IEnumerable<T>> Add<T>(List<T> items) where T : class, new();
        Configuration RegionConfigureationAsync(int regionID);
        Task<T> Update<T>(Int32 pkId, T item) where T : class, new();
        Task Delete<T>(T item) where T : class, new();
        IQueryable<Role> GetRoles();
        IBasicUser GetUserByUsername(string username);
        Wateruse GetWateruse(List<string> sources, Int32 startyear, Int32? endyear);
        Wateruse GetWateruse(object basin, Int32 startyear, Int32? endyear);
        Region GetRegionByIDOrShortName(string identifier);
        IDictionary<string, Wateruse> GetWaterusebySource(List<string> sources, Int32 startyear, Int32? endyear);
        IDictionary<string,Wateruse> GetWaterusebySource(object basin, int startyear, int? endyear);
    }
    public class WaterUseServiceAgent : DBAgentBase, IWaterUseAgent, IBasicUserAgent
    {
        public bool IncludePermittedWithdrawals { private get; set; }
        private Domestic DomesticUse { get; set; }     

        public WaterUseServiceAgent(WaterUseDBContext context) : base(context) {

            //optimize query for disconnected databases.
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        #region Configureation
        public Configuration RegionConfigureationAsync(int regionID)
        {
            try
            {
                var sourcequery = Select<Source>().Where(s => s.RegionID == regionID);

                return new Configuration()
                            {
                                HasPermits = sourcequery.Any(s => s.Permits.Count > 0),
                                HasReturns = Select<CatagoryCoefficient>().Any(cc => cc.RegionID == regionID),
                                MaxYear = sourcequery.Include(s => s.TimeSeries).SelectMany(s => s.TimeSeries.Select(t => t.Date)).Max().Year,
                                MinYear = sourcequery.Include(s => s.TimeSeries).SelectMany(s => s.TimeSeries.Select(t => t.Date)).Min().Year,
                                Units = "MGD"
                            };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
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
        public IBasicUser GetUserByUsername(string username) {
            try
            {

                return Select<Manager>().Include(p => p.Role).Where(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase))
                    .Select(u => new User() { FirstName = u.FirstName, LastName = u.LastName,
                     Email = u.Email, Role= u.Role.Name, RoleID = u.RoleID, ID= u.ID, Username = u.Username, Salt=u.Salt, password = u.Password} ).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }


        }
        #endregion
        #region Region
        public Region GetRegionByIDOrShortName(string identifier)
        {
            try
            {

                return  Select<Region>().FirstOrDefault(e => String.Equals(e.ID.ToString().Trim().ToLower(),
                                                         identifier.Trim().ToLower()) || String.Equals(e.ShortName.Trim().ToLower(),
                                                         identifier.Trim().ToLower()));
            }
            catch (Exception ex)
            {
                sm(WiM.Resources.MessageType.error, "Error finding region " + ex.Message);
                return null;
            }


        }
        #endregion
        #region Source
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

        public void ComputeDomesticWateruse() {

            this.DomesticUse = new Domestic()
            {
                GroundWater = 1.728,
                SurfaceWater = 0.035
            };
        }
        #endregion
 

        #region HELPER METHODS
        private Wateruse getAggregatedWaterUse(IQueryable<Source> sources, Int32 startyear, Int32? endyear)
        {
            List<Source> sourceList = null;
            try
            {
                if (IncludePermittedWithdrawals) sources = sources.Include(s => s.Permits).Include("Permits.UnitType");
                sourceList = sources.Include(s => s.SourceType).Include("TimeSeries.UnitType").Include(s => s.CatagoryType).ToList();
                
                return new Wateruse()
                {                    
                    ProcessDate = DateTime.Now,
                    StartYear = startyear,
                    EndYear = endyear,
                    Return = getWaterUseReturns(sourceList.ToList(), startyear, endyear),
                    Withdrawal = getWaterUseWithdrawals(sourceList.ToList(), startyear, endyear)
                };
            }   
            catch (Exception)
            {
                throw;
            }
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
                            //Bysources doesn't return by catagories
                            //item.CatagoryTypeID = null;
                            var wu = new Wateruse()
                            {
                                ProcessDate = DateTime.Now,
                                StartYear = startyear,
                                EndYear = endyear,
                                Return = getWaterUseReturns(sourceList.Where(s => s.Equals(item)).ToList(), startyear, endyear),
                                Withdrawal = getWaterUseWithdrawals(sourceList.Where(s => s.Equals(item)).ToList(), startyear, endyear)
                            };
                            if(wu.Return != null || wu.Withdrawal != null)
                                result.Add(item.FacilityCode,wu);

                        }//next item
                        return result;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

        private WateruseSummary getWaterUseWithdrawals(IList<Source> sources, Int32 startyear, Int32? endyear)
        {
            List<TimeSeries> tslist = null;
            Int32 yrspan = 1;
            List<Permit> pmtlist = null;

            try
            {
                if (sources.Count() < 1) return null;

                if (!endyear.HasValue) endyear = startyear;
                yrspan = (endyear.Value + 1) - startyear;

                tslist = sources.SelectMany(s => s.TimeSeries).Where(ts => ts.Date.Year >= startyear && ts.Date.Year <= endyear.Value).ToList();
                if (this.IncludePermittedWithdrawals)
                    pmtlist = sources.Where(s=>s.Permits !=null).SelectMany(s => s.Permits).Where(p => p.StartDate.HasValue && p.StartDate.Value.Year >= startyear && 
                                                                                p.StartDate.Value.Year <= endyear.Value).ToList();
                if (this.DomesticUse != null)
                    tslist.AddRange(getDomesticTimeseries(startyear,endyear.Value));

                if (tslist.Count < 1) return null;

                return new WateruseSummary() {
                    Annual = tslist != null && tslist.Count > 0 ? tslist.GroupBy(ts => ts.Source.SourceType.Code)
                    .ToDictionary(ky => ky.Key, mval => new WateruseValue() {
                        Name = mval.First().Source.SourceType.Name,
                        Description = "Daily Annual Average " + mval.First().Source.SourceType.Description,
                        Value = mval.Sum(ts => ts.Value * DateTime.DaysInMonth(ts.Date.Year, ts.Date.Month) / getDaysInYear(ts.Date.Year))/yrspan,
                        Unit = mval.First().UnitType
                    }):null,

                    Monthly = tslist != null && tslist.Count > 0 ? tslist.GroupBy(ts => ts.Date.Month)
                    .ToDictionary(ky => ky.Key, mval => new MonthlySummary()
                    {
                        Month = mval.GroupBy(cd => cd.Source.SourceType.Code)
                        .ToDictionary(ky => ky.Key, cval => new WateruseValue() {
                            Name = cval.First().Date.ToString("MMM", CultureInfo.InvariantCulture) +" "+ cval.First().Source.SourceType.Name,
                            Description = cval.First().Date.ToString("MMM", CultureInfo.InvariantCulture) + " daily monthly average",
                            Unit = cval.First().UnitType,
                            Value = cval.Sum(ts => ts.Value / yrspan)
                        }),
                        Code = mval.Any(i => i.Source.CatagoryTypeID.HasValue) ? mval.Where(cd=>cd.Source.CatagoryTypeID.HasValue).GroupBy(cd => cd.Source.CatagoryType.Code)
                        .ToDictionary(ky => ky.Key, cval => new WateruseValue()
                        {
                            Name = cval.First().Source.CatagoryType.Name,
                            Description = "Daily " + cval.First().Date.ToString("MMM", CultureInfo.InvariantCulture) + " average " + cval.First().Source.CatagoryType.Name,
                            Unit = cval.First().UnitType,
                            Value = cval.Sum(ts => ts.Value / yrspan)
                        }) : null
                    }):null,

                    Permitted = pmtlist !=null && pmtlist.Count > 0 ? new Dictionary<string, WateruseValue>() {
                        { "Well", new WateruseValue(){
                                        Name = "Permitted " + pmtlist.First().Source.SourceType.Name,
                                        Description = "Daily Annual Average Permitted" + pmtlist.First().Source.SourceType.Description,
                                        Value = pmtlist.Where(p=>p.WellCapacity.HasValue).Sum(p => p.WellCapacity.Value * DateTime.DaysInMonth(p.StartDate.Value.Year, p.StartDate.Value.Month) / getDaysInYear(p.StartDate.Value.Year)),
                                        Unit = pmtlist.First().UnitType
                                    }
                        },
                        { "Intake",new WateruseValue(){
                                        Name = "Permitted " + pmtlist.First().Source.SourceType.Name,
                                        Description = "Daily Annual Average Permitted" + pmtlist.First().Source.SourceType.Description,
                                        Value = pmtlist.Where(p => p.IntakeCapacity.HasValue).Sum(p => p.IntakeCapacity.Value * DateTime.DaysInMonth(p.StartDate.Value.Year, p.StartDate.Value.Month) / getDaysInYear(p.StartDate.Value.Year)),
                                        Unit = pmtlist.First().UnitType
                                    }
                        }
                    } : null                     
                };
            }
            catch (Exception ex)
            {
                sm(WiM.Resources.MessageType.error, "Error computing WU: " + ex.Message);
                return null;
            }
        }

        private WateruseSummary getWaterUseReturns(List<Source> sources, int startyear, int? endyear)
        {
            List<NetTimeSeries> tslist = null;
            Int32 yrspan = 1;
            List<CatagoryCoefficient> catCoeff = null;

            try
            {
                if (sources.Count() < 1) return null;

                if (!endyear.HasValue) endyear = startyear;
                yrspan = (endyear.Value + 1) - startyear;
                catCoeff = Select<CatagoryCoefficient>().ToList();

                tslist = sources.All(i => catCoeff.Select(ct => ct.RegionID).Contains(i.RegionID)) ?sources.SelectMany(s => s.TimeSeries).Where(ts => ts.Date.Year >= startyear && ts.Date.Year <= endyear.Value)
                    .Select(ts=>new NetTimeSeries() {
                         Date = ts.Date,
                         ID = ts.ID,
                         SourceID = ts.ID,
                         Source = ts.Source,
                         UnitTypeID = ts.UnitTypeID ,
                         UnitType = ts.UnitType,
                         multiplier = ts.Value*catCoeff.DefaultIfEmpty(new CatagoryCoefficient() { Value=0 }).FirstOrDefault(ct=>ct.RegionID == ts.Source.RegionID && ct.CatagoryTypeID == ts.Source.CatagoryTypeID).Value,
                         Value =ts.Value
                    }).ToList():null;

                if (tslist.Count < 1) return null;

                if (this.DomesticUse != null && sources.All(i => catCoeff.Select(ct => ct.RegionID).Contains(i.RegionID)))
                {
                    //domestic id = 4
                    tslist.AddRange(getDomesticTimeseries(startyear, endyear.Value).Select(ts=>new NetTimeSeries() {
                        Date = ts.Date,
                        ID = ts.ID,
                        SourceID = ts.ID,
                        Source = ts.Source,
                        UnitTypeID = ts.UnitTypeID,
                        UnitType = ts.UnitType,
                        multiplier = ts.Value*catCoeff.Where(ct => ct.CatagoryTypeID == 4).Average(s => s.Value),
                        Value = ts.Value
                    }));
                }

                return new WateruseSummary()
                {
                    Annual = tslist != null && tslist.Count > 0 ? tslist.GroupBy(ts => ts.Source.SourceType.Code)
                    .ToDictionary(ky => ky.Key, mval => new WateruseValue()
                    {
                        Name = mval.First().Source.SourceType.Name.Replace("withdrawal", "return"),
                        Description = "Daily Annual Average " + mval.First().Source.SourceType.Description,
                        Value = (mval.Sum(ts => ts.Value * DateTime.DaysInMonth(ts.Date.Year, ts.Date.Month) / getDaysInYear(ts.Date.Year)) - mval.Sum(ts => ts.multiplier * DateTime.DaysInMonth(ts.Date.Year, ts.Date.Month) / getDaysInYear(ts.Date.Year)))/yrspan,
                        Unit = mval.First().UnitType
                    }) : null,

                    Monthly = tslist != null && tslist.Count > 0 ? tslist.GroupBy(ts => ts.Date.Month)
                    .ToDictionary(ky => ky.Key, mval => new MonthlySummary()
                    {
                        Month = mval.GroupBy(cd => cd.Source.SourceType.Code)
                        .ToDictionary(ky => ky.Key, cval => new WateruseValue()
                        {
                            Name = cval.First().Date.ToString("MMM", CultureInfo.InvariantCulture) + " " + cval.First().Source.SourceType.Name.Replace("withdrawal","return"),
                            Description = cval.First().Date.ToString("MMM", CultureInfo.InvariantCulture) + " daily monthly average",
                            Unit = cval.First().UnitType,
                            Value = cval.Sum(ts => ts.Value / yrspan) - cval.Sum(ts => ts.multiplier / yrspan)
                        })
                    }) : null
                };
            }
            catch (Exception ex)
            {
                sm(WiM.Resources.MessageType.error, "Error computing WU: " + ex.Message);
                return null;
            }
        }

        private IEnumerable<TimeSeries> getDomesticTimeseries(int startyear, int endyear)
        {
            List<TimeSeries> domesticTS = new List<TimeSeries>();
            try
            {

                for (int i = startyear; i <= endyear; i++)
                {
                    if(DomesticUse.GroundWater.HasValue)
                        domesticTS.AddRange(Enumerable.Range(1, 12).Select(r => new TimeSeries()
                        {
                            Date = new DateTime(i, r, 1),
                            UnitType = new UnitType() { Abbreviation = "MGD", Name = "Million Gallons per Day" },
                            Source = new Source() { SourceType = new SourceType() { Code = "GW" }, CatagoryType = new CatagoryType() { Code = "DO", Name = "Domestic" } },
                            Value = DomesticUse.GroundWater.Value
                        }));

                    if (DomesticUse.SurfaceWater.HasValue)
                        domesticTS.AddRange(Enumerable.Range(1, 12).Select(r => new TimeSeries()
                        {
                            Date = new DateTime(i, r, 1),
                            UnitType = new UnitType() { ID=1, Abbreviation = "MGD", Name = "Million Gallons per Day" },
                            Source = new Source() { SourceType = new SourceType() { Code = "SW" }, CatagoryType = new CatagoryType() { Code = "DO", Name = "Domestic" } },
                            Value = DomesticUse.SurfaceWater.Value
                        }));
                }//next yr

                return domesticTS;

            }
            catch (Exception)
            {
                return new List<TimeSeries>();
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
