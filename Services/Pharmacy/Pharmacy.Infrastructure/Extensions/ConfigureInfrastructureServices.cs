﻿using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pharmacy.Application.Contracts.Infrastructure;
using Pharmacy.Infrastructure.Identity;
using System.Security.Cryptography.X509Certificates;

namespace Pharmacy.Infrastructure.Extensions
{
    public static class ConfigureInfrastructureServices
    {
        public static WebApplicationBuilder AddInfrastructureServices(this WebApplicationBuilder builder)
        {
            builder.AddIdentityServerServicesFromAppSettings();
            builder.AddIdentityAuthentication();
            builder.Services.AddScoped<IIdentityService, IdentityService>();

            return builder;
        }

        public static WebApplicationBuilder AddIdentityServerServicesFromAppSettings(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentityServer()
            .AddSigningCredential(new X509Certificate2(Path.Combine("idsrv3test.pfx"), "idsrv3test"))
            .AddInMemoryApiResources(builder.Configuration.GetSection("IdentityServer:ApiResources"))
            .AddInMemoryApiScopes(builder.Configuration.GetSection("IdentityServer:ApiScopes"))
            .AddInMemoryClients(builder.Configuration.GetSection("IdentityServer:Clients"))
            .AddCustomUserStore();
            return builder;
        }

        public static WebApplicationBuilder AddIdentityAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(
                    IdentityServerAuthenticationDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.Authority = builder.Configuration["IdentityServer:Authority"];
                        options.RequireHttpsMetadata = false;
                        options.Audience = builder.Configuration["IdentityServer:ApiName"];
                        options.TokenValidationParameters.ValidateIssuer = false;
                        //options.TokenValidationParameters.ValidIssuers = new[]
                        //{
                        //    builder.Configuration["IdentityServer:ValidIssuer"]
                        //};
                    },
                    null
                );

            return builder;
        }
    }
}
