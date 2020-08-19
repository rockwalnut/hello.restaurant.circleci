using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

//DI
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

//model
using hello.restaurant.api.Models;
//using hello.restaurant.api.Services;
//using hello.restaurant.api.Repositories;
using hello.restaurant.api.APIs.Services;
using Swashbuckle.AspNetCore.Swagger;
using hello.restaurant.api.Middleware;
using hello.restaurant.api.Extensions;
using hello.restaurant.api.Services;
using AutoMapper;
using hello.restaurant.api.Mapper;
using hello.restaurant.api.Providers;
using hello.restaurant.api.Filters;

namespace hello.restaurant.api
{
    public class Startup
    {

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the 
        //container.
        public void ConfigureServices(IServiceCollection services)
        {
            //in memory
            services.AddDbContext<NorthwindContext>(opt =>
                opt.UseInMemoryDatabase("Northwind"));

            //add an APIs Service
            services.AddHttpClient<IGoogleService, GoogleService>().SetHandlerLifetime(TimeSpan.FromMinutes(5));

            //for http request information
            services.AddHttpContextAccessor();

            //http client factory
            //Set 5 min as the lifetime for the HttpMessageHandler objects in the pool used for the Catalog Typed Client 
            //services.AddHttpClient<IClientService, ClientService>()
            //.SetHandlerLifetime(TimeSpan.FromMinutes(5));

            //app service
            services.AddScoped<RestaurantService>();
            services.AddScoped<IRestaurantService, CachedRestaurantService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<ICacheProvider, CacheProvider>();

            // Auto Mapper Configurations

            // Auto Mapper
            //  services.AddAutoMapper(typeof(MappingProfile));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.TryAddSingleton(mapper);

            // services.AddScoped<NorthwindContext>();
            services.AddApiVersioning();

            //enable Cross origin
            services.AddCors(o => o.AddPolicy("AllowCors", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(_configuration.GetValue<string>("AppSettings:CorsUrl", string.Empty))
                    //.WithOrigins(Environment.Configuration.Instance.GetCorsURL())
                    .AllowCredentials();
            }));

            //register memory cache
            services.AddMemoryCache();

            // Add application services.
            //services.AddTransient<IEmailSender, AuthMessageSender>();
            //services.AddTransient<ISmsSender, AuthMessageSender>();

            // Adds Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "v1",
                    Title = "hello.restaurant.api",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "thanyadol",
                        Email = string.Empty,
                        Url = "https://twitter.com/thanyadol"
                    },
                    License = new License
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    }
                });
            });

            //register custom filter
            services.AddScoped<EnsureUserAuthorizeInAsync>();

            // ...
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app.UseAuthentication();
            app.UseCors("AllowCors");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for 
                // production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Adds Swagger
            if (_configuration.GetValue<bool>("AppSettings:Swagger:IsEnabled", false) == true)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "VOT-API V1");
                });
            }

            //for dependency injection service
            app.ApplicationServices.GetService<IDisposable>();

            app.ConfigureCustomExceptionMiddleware();

            //Add our new middleware to the pipeline
            app.UseMiddleware<LoggingMiddleware>();


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
