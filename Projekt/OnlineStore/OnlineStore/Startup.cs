using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace OnlineStore
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
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
            // enable ASP.NET Core MVC
            app.UseMvc(routes => {});
        }
    }
}
