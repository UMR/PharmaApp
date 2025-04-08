using Pharmacy.Domain;

namespace Pharmacy.Application.Contracts.Persistence;

public interface IPackageRepository
{
    Task<Package> GetAsync();
    Task<Package> GetAsync(Guid packageId);
}
