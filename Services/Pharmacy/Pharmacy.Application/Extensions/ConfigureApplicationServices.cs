using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pharmacy.Application.Features.Authentication.Services;
using Pharmacy.Application.Features.Customer.Services;
using Pharmacy.Application.Features.CustomerPharmacy.Services;
using Pharmacy.Application.Features.PharmacyInfo.Services;
using Pharmacy.Application.Features.PharmacyUrls.Services;
using Pharmacy.Application.Features.User.Services;
using Pharmacy.Application.Features.UserLogs.Services;
using Pharmacy.Application.Mappings;
using Serilog;

namespace Pharmacy.Application.Extensions
{
    public static class ConfigureApplicationServices
    {
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.AddSerilogFromAppSettings();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserLoginService, UserLoginService>();
            builder.Services.AddScoped<IPharmacyService, PharmacyService>();
            builder.Services.AddScoped<IPharmacyUrlService, PharmacyUrlService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICustomerPharmacyService, CustomerPharmacyService>();
            
            return builder;
        }

        private static WebApplicationBuilder AddSerilogFromAppSettings(this WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                        .Enrich.FromLogContext()
                        .Enrich.WithMachineName()
                        .Enrich.WithThreadId()
                        .Enrich.WithThreadName()
                        .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddSerilog(logger);

            return builder;
        }
    }
}
