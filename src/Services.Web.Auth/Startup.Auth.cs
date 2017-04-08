using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Farfetch.Services.Web.Auth.CustomTokenProvider;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Farfetch.Services.Web.Auth
{
    public partial class Startup
    {
        private void ConfigureAuth(IApplicationBuilder app)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value));
            
            var tokenProviderOptions = new TokenProviderOptions
            {
                Path = Configuration.GetSection("TokenAuthentication:TokenPath").Value,
                Audience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                Issuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity
            };

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            var jwtBearerOptions = new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme,
                TokenValidationParameters = tokenValidationParameters
            };

            ////jwtBearerOptions.SecurityTokenValidators.Clear();
            ////jwtBearerOptions.SecurityTokenValidators.Add(new TicketDataFormatTokenValidator());

            app.UseJwtBearerAuthentication(jwtBearerOptions);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                AuthenticationScheme = "Cookie",
                CookieName = Configuration.GetSection("TokenAuthentication:CookieName").Value,
                TicketDataFormat = new CustomJwtDataFormat(
                    SecurityAlgorithms.HmacSha256,
                    tokenValidationParameters)
            });

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(tokenProviderOptions));
        }

        private Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            // DEMO CODE, DON NOT USE IN PRODUCTION!!!
            if (username == "TEST" && password == "TEST123")
            {
                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
            }

            // Account doesn't exists
            return Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
