using System.Collections.Generic;
using System.Threading;
using WaterUseAgent.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using WiM.Security.Authentication.Basic;
using WaterUseAgent;
using WaterUseDB.Resources;
using System;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WaterUseServices.XUnitTest
{
    public class InMemoryAgent : IWaterUseAgent
    {
        private List<Role> Roles { get; set; }
        private List<Source> Sources { get; set; }
        public bool IncludePermittedWithdrawals { set => throw new NotImplementedException(); }

        public InMemoryAgent()
        {
            this.Roles = new List<Role>()
            { new Role() { ID=1,Name= "MockTestRole1", Description="test mock role 1" },
                new Role() { ID=2,Name= "MockTestRole2", Description="test mock role 2" }};

        }

        public Task<T> Add<T>(T item) where T : class, new()
        {
            switch (typeof(T).Name)
            {
                case "Role":
                    Roles.Add(item as Role);
                    break;
                case "Source":
                    Sources.Add(item as Source);
                    break;
                default:
                    throw new NotImplementedException(typeof(T).Name);
            }// end switch

            return Task.Run(() => { return item; });
        }

        public Task<IEnumerable<T>> Add<T>(List<T> items) where T : class, new()
        {
            switch (typeof(T).Name)
            {
                case "Role":
                    Roles.AddRange(items.Cast<Role>());
                    return Task.Run(() => { return Roles.Cast<T>(); });
                case "Source":
                    Sources.AddRange(items.Cast<Source>());
                    return Task.Run(() => { return Sources.Cast<T>(); });
                default:
                    throw new NotImplementedException(typeof(T).Name);
            }// end switch
           
        }

        public Task Delete<T>(T item) where T : class, new()
        {
            switch (typeof(T).Name)
            {
                case "Role":
                    return Task.Run(() => { this.Roles.Remove(item as Role); });
                    
                case "Source":
                    return Task.Run(() => { this.Sources.Remove(item as Source); });
                default:
                    throw new NotImplementedException(typeof(T).Name);
            }// end switch
        }

        public Task<T> Find<T>(int pk) where T : class, new()
        {
            switch (typeof(T).Name)
            {
                case "Role":
                    return Task.Run(() => { return Roles.Find(i => i.ID == pk) as T; });
                case "Source":
                    return Task.Run(() => { return Sources.Find(i => i.ID == pk) as T; });
                default:
                    throw new NotImplementedException(typeof(T).Name);
            }// end switch
        }

        public Manager GetManagerByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Role> GetRoles()
        {
            return this.Roles.AsQueryable();
        }

        public IQueryable<T> Select<T>() where T : class, new()
        {
            if (typeof(T) == typeof(Role))
                return this.Roles.AsQueryable() as IQueryable<T>;

            throw new Exception("not of correct type");
        }

        public Task<T> Update<T>(int pkId, T item) where T : class, new()
        {
            if (typeof(T) == typeof(Role))
            {
                var index = this.Roles.FindIndex(x => x.ID == pkId);
                (item as Role).ID = pkId;
                this.Roles[index] = item as Role;
            }
            throw new Exception("not of correct type");
        }

        public Wateruse GetWateruse(List<string> sources, int startyear, int? endyear)
        {
            throw new NotImplementedException();
        }

        public Wateruse GetWateruse(object basin, int startyear, int? endyear)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, Wateruse> GetWaterusebySource(List<string> sources, int startyear, int? endyear)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, Wateruse> GetWaterusebySource(object basin, int startyear, int? endyear)
        {
            throw new NotImplementedException();
        }

        public void ComputeDomesticWateruse()
        {
            throw new NotImplementedException();
        }

        public Configuration RegionConfigureationAsync(int regionID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Source> GetSources(IEnumerable<Region> regions = null, bool removeFCIDcode = true)
        {
            throw new NotImplementedException();
        }

        public Source GetSource(int ID, bool removeFCIDcode = true)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Region> GetManagedRegion(int ManagerID)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Region> GetRegions()
        {
            throw new NotImplementedException();
        }

        public IBasicUser GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Region GetRegionByIDOrShortName(string identifier)
        {
            throw new NotImplementedException();
        }
    }
    public class InMemoryModelValidator : IObjectModelValidator
    {
        public InMemoryModelValidator()
        {
        }
        public void Validate(ActionContext actionContext, ValidationStateDictionary validationState, string prefix, object model)
        {
            //assume all is valid
            return;
        }
    }
}
