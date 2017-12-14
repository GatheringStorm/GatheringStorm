﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GatheringStorm.Api.Data;
using Microsoft.EntityFrameworkCore;
using GatheringStorm.Api.Services;
using GatheringStorm.Api.Models.DB;
using Microsoft.AspNetCore.Identity;
using System;

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

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = Configuration["GatheringStormGoogleClientId"];
                    googleOptions.ClientSecret = Configuration["GatheringStormGoogleClientSecret"];
                })
                .AddJwtBearer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AppDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            dbContext.Database.EnsureCreated();

            app.UseMvc();
        }
    }
}
