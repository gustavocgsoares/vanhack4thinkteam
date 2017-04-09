﻿using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;

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
        /// <param name="app"></param>
        private void ConfigureAuth(IApplicationBuilder app)
        {
            var options = GetJwtBearerOptions();

            app.UseJwtBearerAuthentication(options);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private JwtBearerOptions GetJwtBearerOptions()
        {
            var parameters = GetTokenValidationParameters();
            var options = new JwtBearerOptions();

            options.AutomaticAuthenticate = true;
            options.AutomaticChallenge = true;
            options.AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;
            options.TokenValidationParameters = parameters;

            return options;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private TokenValidationParameters GetTokenValidationParameters()
        {
            var secretKey = Configuration.GetSection("TokenAuthentication:SecretKey").Value;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var parameters = new TokenValidationParameters();

            // The signing key must match!
            parameters.ValidateIssuerSigningKey = true;
            parameters.IssuerSigningKey = signingKey;

            // Validate the JWT Issuer (iss) claim
            parameters.ValidateIssuer = true;
            parameters.ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value;

            // Validate the JWT Audience (aud) claim
            parameters.ValidateAudience = true;
            parameters.ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value;

            // Validate the token expiry
            parameters.ValidateLifetime = true;

            // If you want to allow a certain amount of clock drift, set that here:
            parameters.ClockSkew = TimeSpan.Zero;

            return parameters;
        }
    }
}
