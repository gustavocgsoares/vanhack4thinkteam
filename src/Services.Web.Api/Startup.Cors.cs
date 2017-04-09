using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Farfetch.Services.Web.Api
{
    /// <summary>
    ///
    /// </summary>
    public partial class Startup
    {
        private const string CorsPolicy = "CorsPolicy";

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureCors(IServiceCollection services)
        {
            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials());
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        private void ConfigureCors(IApplicationBuilder app)
        {
            // Global policy, if assigned here (it could be defined indvidually for each controller)
            app.UseCors(CorsPolicy);
        }
    }
}
