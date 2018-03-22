﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore
{
    public class Startup
    {
        // receive details of the configuration data 
        // contained in the appsettings.json file and 
        // use it to configure Entity Framework Core
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // set up Entity Framework Core within the ConfigureServices method
            services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(Configuration["Data:OnlineStoreProducts:ConnectionString"]));
            // when a component (controller) needs an implementation 
            // of the IProductRepository interface, it should receive
            // an instance of the EFProductRepository class
            // (a new EFProductRepository object should be created 
            // each time the IProductRepository interface is needed)
            services.AddTransient<IProductRepository, EFProductRepository>();
            // set up shared objects
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // set up an HTTP request processor

            // display a page that shows detailed information about exceptions
            // **Enable the developer exception page only when the app is running in the Development environment.**
            app.UseDeveloperExceptionPage();
            // provide status code pages such as 404 Not Found
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling
            app.UseStatusCodePages();
            // enable support for serving static content from the wwwroot folder
            app.UseStaticFiles();
            // enable ASP.NET Core MVC;
            // send requests that arrive for the root URL of the application 
            // (http://mysite /) to the List action method in the ProductController class
            app.UseMvc(routes => {
                routes.MapRoute(
                name: "default",
                template: "{controller=Product}/{action=List}/{id?}");
            });
        }
    }
}
