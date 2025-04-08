using Pharmacy.Application.Features.PackageFeature.Dtos;

namespace Pharmacy.Application.Features.PackageFeature.Services;

public interface IPackageService
{
    Task<PackageDto> GetLatestAsync();
    Task<PackageDto> GetAsync(Guid packageId);
}
