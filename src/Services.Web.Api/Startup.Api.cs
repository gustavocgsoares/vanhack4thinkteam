using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace Farfetch.Services.Web.Api
{
    /// <summary>
    ///
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureApi(IServiceCollection services)
        {
            services.AddRouting(opt => opt.LowercaseUrls = true);

            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(new DateTime(2016, 7, 1));
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        private void ConfigureApi(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "api/{version}/{controller}/{id?}");
            });
        }
    }
}
