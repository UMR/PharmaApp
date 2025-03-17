using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Domain;
using Pharmacy.Persistence.Repositories;

namespace Pharmacy.Persistence.Extensions
{
    public static class ConfigurePersistencdServices
    {
        public static WebApplicationBuilder AddPersistenceServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<PharmaAppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUserLoginRepository, UserLoginRepository>();
            builder.Services.AddScoped<IPharmacyRepository, PharmacyRepository>();
            builder.Services.AddScoped<IPharmacyUrlRepository, PharmacyUrlRepository>();

            return builder;
        }
    }
}
