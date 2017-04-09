using Farfetch.Application.Contexts.Corporate;
using Farfetch.Application.Contexts.Product;
using Farfetch.Application.Contexts.Sale;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Application.Interfaces.Product;
using Farfetch.Application.Interfaces.Sale;
using Farfetch.Data.Repositories.Corporate;
using Farfetch.Data.Repositories.Product;
using Farfetch.Data.Repositories.Sale;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Farfetch.Services.Web.Api
{
    /// <summary>
    /// Partial startup class to main configuration.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Hosting environment.
        /// </summary>
        private IHostingEnvironment hostingEnv;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">Hosting environment to specific configuration, if exists.</param>
        public Startup(IHostingEnvironment env)
        {
            hostingEnv = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the configuration root.
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// Main service collection configuration.
        /// </summary>
        /// <param name="services">Service collection to be configured.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICategoryApp, CategoryApp>();
            services.AddTransient<ICustomerApp, CustomerApp>();
            services.AddTransient<IEmployeeApp, EmployeeApp>();
            services.AddTransient<IItemApp, ItemApp>();
            services.AddTransient<IOrderApp, OrderApp>();
            services.AddTransient<IUserApp, UserApp>();

            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            ConfigureLogger(services);
            ConfigureCors(services);
            ConfigureSwagger(services);
            ConfigureApi(services);

            services.Configure<CrossCutting.Configurations.Data>(Configuration.GetSection("Data"));
        }

        /// <summary>
        /// Main application builder configuration.
        /// </summary>
        /// <param name="app">Application builder to be configured.</param>
        /// <param name="env">Hosting environment to specific configuration, if exists.</param>
        /// <param name="loggerFactory">Logger factory to be configured.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles();

            ConfigureLogger(app, loggerFactory);
            ConfigureCors(app);
            ConfigureAuth(app);
            ConfigureSwagger(app);
            ConfigureApi(app);
        }
    }
}
