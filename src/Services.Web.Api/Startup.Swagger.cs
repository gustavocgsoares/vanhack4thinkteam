using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Farfetch.Services.Web.Api
{
    /// <summary>
    /// Partial startup class to docs configuration.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Docs service collection configuration.
        /// </summary>
        /// <param name="services">Service collection to be configured.</param>
        private void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", GetSwaggerInfo());

                c.DescribeAllEnumsAsStrings();
                ////c.OperationFilter<AuthResponsesOperationFilter>();

                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.ControllerAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    var values = apiDesc.RelativePath
                        .Split('/')
                        .Select(v => v.Replace("v{version}", docName));

                    apiDesc.RelativePath = string.Join("/", values);

                    return versions.Any(v => $"v{v.ToString()}" == docName);
                });

                ////// Set the comments path for the swagger json and ui.
                ////var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                ////var xmlPath = Path.Combine(basePath, "Services.Web.Api.xml");
                ////c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Docs application builder configuration.
        /// </summary>
        /// <param name="app">Application builder to be configured.</param>
        private void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            ////app.UseSwaggerUI(options =>
            ////{
            ////    options.RoutePrefix = "docs";
            ////    options.SwaggerEndpoint("../v1/swagger.json", "My API V1");
            ////    ////options.SwaggerEndpoint("/v1/swagger.json", "Farfetch APIs - v1");
            ////    options.EnabledValidator();
            ////    options.BooleanValues(new object[] { 0, 1 });
            ////    options.DocExpansion("full");
            ////    options.SupportedSubmitMethods(new[] { "get", "post", "put", "delete" });
            ////    options.ShowRequestHeaders();
            ////    options.ShowJsonEditor();
            ////    ////options.InjectStylesheet("/swagger-ui/custom.css");
            ////    // Provide client ID, client ID, realm and application name
            ////    ////options.ConfigureOAuth2("swagger-ui", "swagger-ui-secret", "swagger-ui-realm", "Swagger UI");
            ////});
        }

        /// <summary>
        /// Get swagger info.
        /// </summary>
        /// <returns>Swagger info.</returns>
        private Info GetSwaggerInfo()
        {
            return new Info
            {
                Title = "Farfetch APIs - v1",
                Version = "v1",
                Description = "Our APIs are yours",
                TermsOfService = "Knock yourself out",
                Contact = new Contact
                {
                    Name = "We are Developer",
                    Email = "we.are.developer@farfetch.com"
                },
                License = new License
                {
                    Name = "Use under LICX",
                    Url = "http://url.com"
                }
            };
        }

        /// <summary>
        /// Authorization filter.
        /// </summary>
        private class AuthResponsesOperationFilter : IOperationFilter
        {
            /// <summary>
            /// Configure possible response for API with authorize attribute.
            /// </summary>
            /// <param name="operation">Operation to be configured.</param>
            /// <param name="context">Context configuration.</param>
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
