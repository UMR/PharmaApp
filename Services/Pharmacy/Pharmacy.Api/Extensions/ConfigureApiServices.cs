using Microsoft.OpenApi.Models;
using Pharmacy.Api.Constants;
using Pharmacy.Api.Filters;
using Pharmacy.Api.Policy;
using Pharmacy.Application.Common.Constants;
using Pharmacy.Application.Features.CurrentUser.Services;

namespace Pharmacy.Api.Extensions
{
    public static class ConfigureApiServices
    { 
        public static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddSingleton<IAuthorizationHandler, ActivePharmacyHandler>();

            builder.Services.AddControllers(config =>
            {
                config.Filters.Add<ApiExceptionFilterAttribute>();
                config.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().
                    RequireAuthenticatedUser().
                    RequireClaim(builder.Configuration["IdentityServer:ClaimType"], builder.Configuration["IdentityServer:ClaimValue"]).Build()));
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(RoleConstant.Admin, policy => policy.RequireRole(RoleConstant.Admin));
                options.AddPolicy(RoleConstant.Pharmacist, policy => policy.RequireRole(RoleConstant.Pharmacist));
                options.AddPolicy($"Active{RoleConstant.Pharmacist}", policy =>
                {
                    policy.Requirements.Add(new ActivePharmacyRequirement());
                });               
            });

            builder.Services.AddCors(o =>
            {
                o.AddPolicy(ApiConstant.CorsPolicy, policy =>
                {
                    var clients = builder.Configuration.GetSection("IdentityServer:Clients")
                        .Get<List<IdentityServerClient>>();

                    var allowedOrigins = clients?
                        .SelectMany(client => client.AllowedCorsOrigins)
                        .Distinct()
                        .ToArray();

                    if (allowedOrigins != null && allowedOrigins.Any())
                    {
                        policy.WithOrigins(allowedOrigins)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    }
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "PharmaApp Web Api", Version = "v1" });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                };

                x.AddSecurityDefinition("Bearer", jwtSecurityScheme);

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
                });
            });

            return builder;
        }
    }
}
