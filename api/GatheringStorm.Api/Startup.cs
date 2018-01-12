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
using Swashbuckle.AspNetCore.Swagger;
using GatheringStorm.Api.Services.Effects;

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "GatheringStorm", Version = "v1" });
            });

            var connString = Configuration["GatheringStormConnection"];
            services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connString));
            services.AddTransient<IGamesService, GamesService>();
            services.AddTransient<IEffectsService, EffectsService>();
            services.AddTransient<IDestroyEffect, DestroyEffect>();
            services.AddTransient<ICardInitializerService, CardInitializerService>();
            services.AddTransient<IControllerUtility, ControllerUtility>();
            services.AddScoped<ILoginManager, LoginManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AppDbContext dbContext, ICardInitializerService cardInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dbContext.Database.EnsureCreated();
            Task.WaitAll(cardInitializer.InitializeCards());

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GatheringStorm v1");
            });
        }
    }
}
