using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        // receive details of the configuration data 
        // contained in the appsettings.json file and 
        // use it to configure Entity Framework Core
        public Startup(IConfiguration configuration) => 
            Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            // set up Entity Framework Core within the ConfigureServices method
            services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(
                Configuration["Data:OnlineStoreProducts:ConnectionString"]));

            services.AddDbContext<AppIdentityDbContext>(options =>
            options.UseSqlServer(
                Configuration["Data:OnlineStoreIdentity:ConnectionString"]));

            services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders();

            // when a component (controller) needs an implementation 
            // of the IProductRepository interface, it should receive
            // an instance of the EFProductRepository class
            // (a new EFProductRepository object should be created 
            // each time the IProductRepository interface is needed)
            services.AddTransient<IProductRepository, EFProductRepository>();
            // register the order repository as a service
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            // service for the Cart class
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            // use the HttpContextAccessor class when implementations 
            // of the IHttpContextAccessor interface are required
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // set up shared objects
            services.AddMvc();
            // enable cart session (services):
            // set up the in-memory data store
            services.AddMemoryCache();
            // register the services used to access session data
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // set up an HTTP request processor
            
            app.UseDeveloperExceptionPage();
            // provide status code pages such as 404 Not Found
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling
            app.UseStatusCodePages();
            // enable support for serving static content from the wwwroot folder
            app.UseStaticFiles();
            // enable cart session (middleware):
            // allows the session system to automatically associate 
            // requests with sessions when they arrive from the client
            app.UseSession();
            app.UseAuthentication();
            // enable ASP.NET Core MVC;
            // send requests that arrive for the root URL of the application 
            // (http://mysite/) to the List action method in the ProductController class
            app.UseMvc(routes => {

                // the specified page of items from the specified category
                routes.MapRoute(
                    name: null,
                    template: "{category}/Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List" }
                );

                // the specified page of items from all categories
                routes.MapRoute(
                    name: null,
                    template: "Page{productPage:int}",
                    defaults: new { controller = "Product",
                        action = "List", productPage = 1 }
                );

                // the first page of items from a specific category
                routes.MapRoute(
                    name: null,
                    template: "{category}",
                    defaults: new { controller = "Product",
                        action = "List", productPage = 1 }
                );

                // the first page of products from all categories
                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { controller = "Product",
                    action = "List", productPage = 1 }
                );

                routes.MapRoute(
                    name: null, 
                    template: "{controller}/{action}/{id?}"
                );
            });
            // seed the database when the application starts
            SeedData.EnsurePopulated(app);
            IdentitySeedData.EnsurePopulated(app); // for admin account
        }
    }
}
