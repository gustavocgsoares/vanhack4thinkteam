using System;
using System.Linq;
using Farfetch.Application.Contexts.Corporate;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Data.Repositories.Corporate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Farfetch.Services.Web.Api
{
    /// <summary>
    ///
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        ///
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmployeeApp, EmployeeApp>();
            services.AddTransient<IUserApp, UserApp>();

            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            ////Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            ////Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials());
            });

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", GetSwaggerInfo());

                ////var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "MyApi.xml");
                ////c.IncludeXmlComments(filePath);
                c.DescribeAllEnumsAsStrings();
                c.OperationFilter<AuthResponsesOperationFilter>();

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });
            });

            ////services.AddSingleton(new MongoClient(Configuration.GetSection("MongoDb:ConnectionString").Value));

            services.Configure<CrossCutting.Configurations.Data>(Configuration.GetSection("Data"));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            ////app.UseApplicationInsightsRequestTelemetry();

            ////app.UseApplicationInsightsExceptionTelemetry();

            ////global policy, if assigned here (it could be defined indvidually for each controller)
            app.UseCors("CorsPolicy");
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "docs";
                c.SwaggerEndpoint("/v1/swagger.json", "My API V1");
                c.EnabledValidator();
                c.BooleanValues(new object[] { 0, 1 });
                c.DocExpansion("full");
                c.SupportedSubmitMethods(new[] { "get", "post", "put", "delete" });
                c.ShowRequestHeaders();
                c.ShowJsonEditor();
                ////c.InjectStylesheet("/swagger-ui/custom.css");
                // Provide client ID, client ID, realm and application name
                ////c.ConfigureOAuth2("swagger-ui", "swagger-ui-secret", "swagger-ui-realm", "Swagger UI");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "api/{version}/{controller}/{id?}");
            });
        }

        private Info GetSwaggerInfo()
        {
            return new Info
            {
                Title = "My API - v1",
                Version = "v1",
                Description = "A sample API to demo Swashbuckle",
                TermsOfService = "Knock yourself out",
                Contact = new Contact
                {
                    Name = "We are Developer",
                    Email = "we.are.developer@tempuri.org"
                },
                License = new License
                {
                    Name = "Apache 2.0",
                    Url = "http://www.apache.org/licenses/LICENSE-2.0.html"
                }
            };
        }

        /// <summary>
        ///
        /// </summary>
        public class AuthResponsesOperationFilter : IOperationFilter
        {
            /// <summary>
            ///
            /// </summary>
            /// <param name="operation"></param>
            /// <param name="context"></param>
            public void Apply(Operation operation, OperationFilterContext context)
            {
                var authAttributes = context.ApiDescription
                    .ControllerAttributes()
                    .Union(context.ApiDescription.ActionAttributes())
                    .OfType<AuthorizeAttribute>();

                if (authAttributes.Any())
                {
                    operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                }
            }
        }
    }
}
