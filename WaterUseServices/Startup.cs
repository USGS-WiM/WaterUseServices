using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WaterUseDB;
using WaterUseAgent;
using WaterUseServices.Security.Authentication.Basic;

namespace WaterUseServices
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
            if (env.IsDevelopment()) {
                builder.AddUserSecrets<Startup>();
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }//end startup       

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<WaterUseDBContext>(options=>
                                                        options.UseNpgsql(String.Format(Configuration
                                                            .GetConnectionString("WaterUseConnection"),Configuration["dbuser"], Configuration["dbpassword"], Configuration["dbHost"]))
                                                            .EnableSensitiveDataLogging());

            services.AddScoped<IWaterUseAgent, WaterUseServiceAgent>();
            services.AddAuthorization(options =>loadAutorizationPolicies(options));
            services.AddMvc();            
        }       

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseBasicAuthentication(new BasicAuthenticationOptions());
            app.UseMvc();
        }

        #region Helper Methods
        private void loadAutorizationPolicies(AuthorizationOptions options)
        {   
            options.AddPolicy(
                "CanModify",
                policy => policy.RequireRole("Administrator", "Manager"));
            options.AddPolicy(
                "Restricted",
                policy => policy.RequireRole("Administrator", "Manager"));
            options.AddPolicy(
                "AdminOnly",
                policy => policy.RequireRole("Administrator"));
        }
        #endregion


    }
}
