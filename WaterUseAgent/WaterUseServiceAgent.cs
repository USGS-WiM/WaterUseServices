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
using WaterUseAgent.Extensions;


namespace WaterUseAgent
{
    public interface IWaterUseAgent
    {
        Boolean IncludePermittedWithdrawals {set; }
        Boolean ComputeReturnsUsingConsumtiveUseCoefficients { set; }
        void ComputeDomesticWateruse(object basin);
        IQueryable<T> Select<T>() where T : class, new();
        Task<T> Find<T>(Int32 pk) where T : class, new();
        Task<T> Add<T>(T item) where T : class, new();
        Task<IEnumerable<T>> Add<T>(List<T> items) where T : class, new();
        Configuration RegionConfigureationAsync(int regionID);
        Task<T> Update<T>(Int32 pkId, T item) where T : class, new();
        Task Delete<T>(T item) where T : class, new();
        Task Delete<T>(Int32 id) where T : class, new();
        IEnumerable<Source> GetSources(IEnumerable<Region> regions = null, bool removeFCIDcode=true);
        Source GetSource(Int32 ID, bool removeFCIDcode=true);
        Source Add(Source item);
        IEnumerable<Source> Add(List<Source> items);
        Source Update(Int32 pkId, Source item);

        IQueryable<Region> GetManagedRegion(Int32 ManagerID);
        IQueryable<Region> GetRegions();

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
        public bool ComputeReturnsUsingConsumtiveUseCoefficients { private get; set; }
        private Domestic DomesticUse { get; set; }     

        public WaterUseServiceAgent(WaterUseDBContext context) : base(context) {
            this.IncludePermittedWithdrawals = false;

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
                                CanComputeReturns = Select<CategoryCoefficient>().Any(cc => cc.RegionID == regionID),
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
        public IQueryable<Region> GetManagedRegion(Int32 ManagerID)
        {
            IQueryable<Region> regionquery = null;
            try
            {
                regionquery = GetRegions();
                regionquery = regionquery.Include(s => s.RegionManagers).Where(s => s.RegionManagers.Any(rm => rm.ManagerID == ManagerID));

                return regionquery;
            }
            catch (Exception ex)
            {
                sm(WiM.Resources.MessageType.error, "Error querying Managed Region " + ex.Message);
                throw;
            }
        }
        public IQueryable<Region> GetRegions()
        {
            try
            {
                return base.Select<Region>();
            }
            catch (Exception ex)
            {
                sm(WiM.Resources.MessageType.error, "Error querying Region " + ex.Message);
                throw;
            }
        }
        #endregion
        #region Source
        public IEnumerable<Source> GetSources(IEnumerable<Region> regions = null, bool removeFCIDcode = true)
        {
            IQueryable<Source> query = null;
            try
            {
                var regionID = regions.ToList().Select(i => i.ID);
                query = base.Select<Source>();
                if (regions != null)
                    query = query.Where(s=>regionID.Contains(
                        s.RegionID));

                if (removeFCIDcode)
                    query = query.AsEnumerable().RemoveFCFIPCode<Source>().AsQueryable();
                return query;

            }
            catch (Exception ex)
            {
                sm(WiM.Resources.MessageType.error, "Error querying source "+ex.Message);
                throw;
            } 
        }
        public Source GetSource(Int32 ID, bool removeFCIDcode)
        {
            var source = base.Find<Source>(ID).Result;
            if (removeFCIDcode) source?.RemoveFCFIPCode();
            return source;
        }
        public IEnumerable<Source> Add(List<Source> items)
        {
            items.ForEach(i=>i.AddFCFIPCode(GetRegionByIDOrShortName(i.RegionID.ToString()).FIPSCode));
            return base.Add<Source>(items).Result.RemoveFCFIPCode();
        }
        public Source Add(Source item)
        {

            item.AddFCFIPCode(GetRegionByIDOrShortName(item.RegionID.ToString()).FIPSCode);

            return base.Add<Source>(item).Result.RemoveFCFIPCode();
        }
        public Source Update(Int32 pkId, Source item)
        {

            item.AddFCFIPCode(GetRegionByIDOrShortName(item.RegionID.ToString()).FIPSCode);

            return base.Update<Source>(pkId, item).Result.RemoveFCFIPCode();
        }
        public Task Delete<T>(Int32 id) where T : class, new() 
        {
            var entity = base.Find<T>(id).Result;
            if (entity == null) return new Task(null);
            return base.Delete<T>(entity);
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

        public void ComputeDomesticWateruse(object basin) {
            //https://github.com/aspnet/EntityFrameworkCore/issues/7810#issuecomment-384909854
            try
            {
                using (var command = this.context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = String.Format(getSQLStatement(sqlTypes.e_domesticsummarystats), basin, 4326);
                    context.Database.OpenConnection();
                    using (System.Data.Common.DbDataReader reader = command.ExecuteReader())
                    {
                        this.DomesticUse = new Domestic();
                        while (reader.Read())
                        {
                            if (reader["name"].ToString() == "gw") this.DomesticUse.GroundWater = Convert.ToDouble(reader["value"]);
                            if (reader["name"].ToString() == "sw") this.DomesticUse.SurfaceWater = Convert.ToDouble(reader["value"]);
                        }
                    }//end using  
                }//end using 
            }
            catch (Exception ex)
            {
                this.DomesticUse = null;
                return;
            }
            finally
            {
                if(context.Database.GetDbConnection().State == System.Data.ConnectionState.Open)
                    context.Database.CloseConnection();
            }
        }
        #endregion
 
        #region HELPER METHODS
        private Wateruse getAggregatedWaterUse(IQueryable<Source> sources, Int32 startyear, Int32? endyear)
        {
            List<Source> sourceList = null;
            try
            {
                if (IncludePermittedWithdrawals) sources = sources.Include(s => s.Permits).Include("Permits.UnitType");
                sourceList = sources.Include(s => s.SourceType).Include("TimeSeries.UnitType").Include(s => s.CategoryType).Include(s=>s.UseType).ToList();
                
                return new Wateruse()
                {                    
                    ProcessDate = DateTime.Now,
                    StartYear = startyear,
                    EndYear = endyear,
                    Return = !ComputeReturnsUsingConsumtiveUseCoefficients ? getWaterUseSummary(sourceList.Where(x => String.Equals(x.UseType.Name, 
                                                                "Return", StringComparison.OrdinalIgnoreCase)).ToList(), startyear, endyear): computeWaterUseSummaryReturns(sourceList,startyear,endyear),
                    Withdrawal = getWaterUseSummary(sourceList.Where(x=>String.Equals(x.UseType.Name,
                                                                "Withdrawal",StringComparison.OrdinalIgnoreCase)).ToList(), startyear, endyear)
                };
            }   
            catch (Exception ex)
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
                        sourceList = sources.Include(s => s.SourceType).Include("TimeSeries.UnitType").Include(s => s.CategoryType).Include(s => s.UseType).ToList();
                        foreach (var item in sourceList)
                        {
                            var wu = new Wateruse()
                            {
                                ProcessDate = DateTime.Now,
                                StartYear = startyear,
                                EndYear = endyear,
                                Return = !ComputeReturnsUsingConsumtiveUseCoefficients ? getWaterUseSummary(sourceList.Where(x => String.Equals(x.UseType.Name, 
                                                                        "Return", StringComparison.OrdinalIgnoreCase))
                                                                    .Where(s => s.Equals(item)).ToList(), startyear, endyear):computeWaterUseSummaryReturns(sourceList.Where(s => s.Equals(item)).ToList(), startyear,endyear),
                                Withdrawal = getWaterUseSummary(sourceList.Where(x => String.Equals(x.UseType.Name, 
                                                                        "Withdrawal", StringComparison.OrdinalIgnoreCase))
                                                                   .Where(s => s.Equals(item)).ToList(), startyear, endyear)
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

        private WateruseSummary getWaterUseSummary(IList<Source> sources, Int32 startyear, Int32? endyear)
        {
            List<TimeSeries> tslist = null;
            
            List<Permit> pmtlist = null;
            //Int32 yrspan = 1;
            try
            {
                if (sources.Count() < 1) return null;

                if (!endyear.HasValue) endyear = startyear;

                tslist = sources.SelectMany(s => s.TimeSeries).Where(ts => ts.Date.Year >= startyear && ts.Date.Year <= endyear.Value).ToList();
                if (this.IncludePermittedWithdrawals)
                    pmtlist = sources.Where(s=>s.Permits !=null).SelectMany(s => s.Permits).Where(p => p.StartDate.HasValue && p.StartDate.Value.Year >= startyear && 
                                                                                p.StartDate.Value.Year <= endyear.Value).ToList();
                if (this.DomesticUse != null)
                    tslist.AddRange(getDomesticTimeseries(startyear,endyear.Value));

                if (tslist.Count < 1) return null;
                //yrspan = tslist.Select(x => x.Date.Year).Distinct().Count();
                return new WateruseSummary() {
                    Annual = tslist != null && tslist.Count > 0 ? tslist.GroupBy(ts => ts.Source.SourceType.Code)
                    .ToDictionary(ky => ky.Key, mval => new WateruseValue() {
                        Name = mval.First().Source.SourceType.Name,
                        Description = "Daily Annual Average " + mval.First().Source.SourceType.Description,
                        Value = mval.Sum(ts => ts.Value * DateTime.DaysInMonth(ts.Date.Year, ts.Date.Month) / getDaysInYear(ts.Date.Year))/mval.Select(x=>x.Date.Year).Distinct().Count(),
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
                            Value = cval.Sum(ts => ts.Value/ mval.Select(x => x.Date.Year).Distinct().Count())
                        }),
                        Code = mval.Any(i => i.Source.CategoryType != null) ? mval.Where(cd=>cd.Source.CategoryType != null).GroupBy(cd => cd.Source.CategoryType.Code)
                        .ToDictionary(ky => ky.Key, cval => new WateruseValue()
                        {
                            Name = cval.First().Source.CategoryType.Name,
                            Description = "Daily " + cval.First().Date.ToString("MMM", CultureInfo.InvariantCulture) + " average " + cval.First().Source.CategoryType.Name,
                            Unit = cval.First().UnitType,
                            Value = cval.Sum(ts => ts.Value/ mval.Select(x => x.Date.Year).Distinct().Count())
                        }) : null
                    }):null,

                    Permitted = pmtlist !=null && pmtlist.Count > 0 ? new Dictionary<string, WateruseValue>() {
                        { "Well", new WateruseValue(){
                                        Name = "Permitted " + pmtlist.First().Source.SourceType.Name,
                                        Description = "Daily Annual Average Permitted" + pmtlist.First().Source.SourceType.Description,
                                        Value = pmtlist.Where(p=>p.WellCapacity.HasValue).Sum(p => p.WellCapacity.Value),
                                        Unit = pmtlist.First().UnitType
                                    }
                        },
                        { "Intake",new WateruseValue(){
                                        Name = "Permitted " + pmtlist.First().Source.SourceType.Name,
                                        Description = "Daily Annual Average Permitted" + pmtlist.First().Source.SourceType.Description,
                                        Value = pmtlist.Where(p => p.IntakeCapacity.HasValue).Sum(p => p.IntakeCapacity.Value),
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

        private WateruseSummary computeWaterUseSummaryReturns(List<Source> sources, int startyear, int? endyear)
        {
            List<NetTimeSeries> tslist = null;
            Int32 yrspan = 1;
            List<CategoryCoefficient> catCoeff = null;

            try
            {
                if (sources.Count() < 1) return null;

                if (!endyear.HasValue) endyear = startyear;
                catCoeff = Select<CategoryCoefficient>().ToList();

                tslist = sources.All(i => catCoeff.Select(ct => ct.RegionID).Contains(i.RegionID)) ?sources.SelectMany(s => s.TimeSeries).Where(ts => ts.Date.Year >= startyear && ts.Date.Year <= endyear.Value)
                    .Select(ts=>new NetTimeSeries() {
                         Date = ts.Date,
                         ID = ts.ID,
                         SourceID = ts.ID,
                         Source = ts.Source,
                         UnitTypeID = ts.UnitTypeID ,
                         UnitType = ts.UnitType,
                         multiplier = ts.Value*catCoeff.DefaultIfEmpty(new CategoryCoefficient() { Value=0 }).FirstOrDefault(ct=>ct.RegionID == ts.Source.RegionID && ct.CategoryTypeID == ts.Source.CategoryTypeID).Value,
                         Value =ts.Value
                    }).ToList():null;

                if (tslist.Count < 1) return null;
                yrspan = tslist.Select(x => x.Date.Year).Distinct().Count();
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
                        multiplier = ts.Value*catCoeff.Where(ct => ct.CategoryTypeID == 4).Average(s => s.Value),
                        Value = ts.Value
                    }));
                }

                return new WateruseSummary()
                {
                    Annual = tslist != null && tslist.Count > 0 ? tslist.GroupBy(ts => ts.Source.SourceType.Code)//gw/sw
                    .ToDictionary(ky => ky.Key, mval => new WateruseValue()
                    {
                        Name = mval.First().Source.SourceType.Name.Replace("withdrawal", "return"),
                        Description = "Daily Annual Average " + mval.First().Source.SourceType.Description,
                        Value = (mval.Sum(ts => ts.Value * DateTime.DaysInMonth(ts.Date.Year, ts.Date.Month) / getDaysInYear(ts.Date.Year))
                                                - mval.Sum(ts => ts.multiplier * DateTime.DaysInMonth(ts.Date.Year, ts.Date.Month) / getDaysInYear(ts.Date.Year))) / yrspan,
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
                    if (DomesticUse.GroundWater.HasValue)
                        domesticTS.AddRange(Enumerable.Range(1, 12).Select(r => new TimeSeries()
                        {
                            Date = new DateTime(i, r, 1),
                            UnitType = new UnitType() { Abbreviation = "MGD", Name = "Million Gallons per Day" },
                            Source = new Source() { UseType = new UseType() { Name= "Withdrawal" }, SourceType = new SourceType() { Code = "GW" }, CategoryType = new CategoryType() { Code = "DO", Name = "Domestic" } },
                            Value = DomesticUse.GroundWater.Value
                        }));

                    if (DomesticUse.SurfaceWater.HasValue)
                        domesticTS.AddRange(Enumerable.Range(1, 12).Select(r => new TimeSeries()
                        {
                            Date = new DateTime(i, r, 1),
                            UnitType = new UnitType() { ID = 1, Abbreviation = "MGD", Name = "Million Gallons per Day" },
                            Source = new Source() { UseType = new UseType() { Name = "Withdrawal" }, SourceType = new SourceType() { Code = "SW" }, CategoryType = new CategoryType() { Code = "DO", Name = "Domestic" } },
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
                                    st_makevalid(
                                        st_transform(
                                            st_setsrid(
                                                ST_GeomFromGeoJSON('{{{0}}}'),
                                            {1}),
                                        4269)))";
                case sqlTypes.e_source:
                    return @"SELECT * FROM ""public"".""Sources"" 
                                WHERE ""ID"" IN ('{0}') OR 
                                LOWER(""FacilityCode"") IN ('{1}')";

                case sqlTypes.e_domesticsummarystats:
                    /*
                    //https://postgis.net/docs/RT_ST_SummaryStats.html
                    //https://gis.stackexchange.com/questions/260274/get-st-summarystats-from-raster-table-using-geojson-polygon
                    return @"WITH clip(geom) AS
                            (SELECT
                                (ST_Transform(
                                    ST_SetSRID(
                                        ST_GeomFromGeoJSON('{0}'),
                                    {1}),
                                 4236))
                            )
                            SELECT SUBSTRING(filename,6,2) as name, 
                            SUM ((ST_SummaryStats(ST_Clip(rast, geom, true))).SUM) as value
                            FROM ""DomesticWateruseRasters"", clip
                                    WHERE ST_Intersects(rast, geom) 
                            GROUP BY filename;";
                            */
                    return @"
                            SELECT
                                SUBSTRING(filename,6,2) as name,
                                (st_summarystats(st_clip(ST_Union(rast), st_transform(
				                                st_setsrid(st_geomfromgeojson('{0}'),
                                                {1}), 4236)), TRUE)).SUM as value

                                FROM ""DomesticWateruseRasters""
                                Where st_intersects(rast, st_transform(
                                                st_setsrid(st_geomfromgeojson('{0}'),
				                                {1}), 4236))
                                GROUP BY filename;";
                default:
                    throw new Exception("No sql for table " + type);
            }
        }
        #endregion
        #region Enumerations
        private enum sqlTypes
        {
            e_sourcebygeojson,
            e_source,
            e_domesticsummarystats
        }
        #endregion
    }
}
