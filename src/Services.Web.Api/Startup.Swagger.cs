using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

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
        private void ConfigureSwagger(IServiceCollection services)
        {
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
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="app"></param>
        private void ConfigureSwagger(IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs";
                options.SwaggerEndpoint("/v1/swagger.json", "My API V1");
                options.EnabledValidator();
                options.BooleanValues(new object[] { 0, 1 });
                options.DocExpansion("full");
                options.SupportedSubmitMethods(new[] { "get", "post", "put", "delete" });
                options.ShowRequestHeaders();
                options.ShowJsonEditor();
                ////options.InjectStylesheet("/swagger-ui/custom.css");
                // Provide client ID, client ID, realm and application name
                ////options.ConfigureOAuth2("swagger-ui", "swagger-ui-secret", "swagger-ui-realm", "Swagger UI");
            });
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
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
        private class AuthResponsesOperationFilter : IOperationFilter
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
