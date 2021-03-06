using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace WaterUseDB.Test
{
    [TestClass]
    public class WaterUseDBTest
    {
        private string connectionstring = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build().GetConnectionString("wateruseConnection");

        [TestMethod]
        public void ConnectionTest()
        {
            using (WaterUseDBContext context = new WaterUseDBContext(new DbContextOptionsBuilder<WaterUseDBContext>().UseNpgsql(this.connectionstring).Options))
            {
                try
                {
                    if (!(context.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists()) throw new Exception("db does ont exist");
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(false, ex.Message);
                }
            }
        }
        [TestMethod]
        public void QueryTest()
        {
            using (WaterUseDBContext context = new WaterUseDBContext(new DbContextOptionsBuilder<WaterUseDBContext>().UseNpgsql(this.connectionstring).Options))
            {
                try
                {
                    var testQuery = context.Roles.ToList();
                    Assert.IsNotNull(testQuery, testQuery.Count.ToString());
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(false, ex.Message);
                }
                finally
                {
                }

            }
        }
    }
}
