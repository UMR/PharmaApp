using Pharmacy.Domain;

namespace Pharmacy.Application.Contracts.Persistence;

public interface IPharmacyUrlRepository
{
    Task<bool> AddAsync(PharmacyUrl pharmacyUrl);
    Task<PharmacyUrl?> GetAsync(Guid pharmacyId);
    Task<PharmacyUrl?> GetAsync(string url);
}
