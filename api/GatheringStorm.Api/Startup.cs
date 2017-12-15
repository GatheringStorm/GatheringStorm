using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GatheringStorm.Api.Data;
using Microsoft.EntityFrameworkCore;
using GatheringStorm.Api.Services;
using GatheringStorm.Api.Models.DB;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using GatheringStorm.Api.Auth;
using GatheringStorm.Api.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace GatheringStorm.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connString = Configuration["GatheringStormConnection"];
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connString));
            services.AddTransient<IGamesService, GamesService>();
            services.AddTransient<IControllerUtility, ControllerUtility>();
            services.AddScoped<ILoginManager, LoginManager>();

            //services.AddAuthentication(o =>
            //    {
            //        o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddCookie()
            //    .AddJwtBearer(o =>
            //    {
            //        o.RequireHttpsMetadata = false;

            //        o.Audience = "24931599658-o9q66rbqprq0lcgrtlbfhcs3kcfqs8rg.apps.googleusercontent.com";
            //        o.Authority = "accounts.google.com";
            //        o.MetadataAddress = "https://accounts.google.com/.well-known/openid-configuration";
            //    })
            //    .AddGoogle(googleOptions =>
            //    {
            //        googleOptions.ClientId = Configuration["GatheringStormGoogleClientId"];
            //        googleOptions.ClientSecret = Configuration["GatheringStormGoogleClientSecret"];
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AppDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseAuthentication();

            dbContext.Database.EnsureCreated();

            app.UseMvc();
        }
    }
}
