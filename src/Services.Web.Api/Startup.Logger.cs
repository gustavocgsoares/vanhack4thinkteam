using System;
using System.Linq;
using System.Text;
using Farfetch.Application.Contexts.Corporate;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Data.Repositories.Corporate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

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
        private void ConfigureLogger(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loggerFactory"></param>
        private void ConfigureLogger(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            ////app.UseApplicationInsightsRequestTelemetry();
            ////app.UseApplicationInsightsExceptionTelemetry();
        }
    }
}
