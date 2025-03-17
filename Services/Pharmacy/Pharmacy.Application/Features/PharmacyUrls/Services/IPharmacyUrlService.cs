using Pharmacy.Domain;

namespace Pharmacy.Application.Features.PharmacyUrls.Services;

public interface IPharmacyUrlService
{
    Task<PharmacyUrl> GetAsync(Guid pharmacyId);
    Task<PharmacyUrl?> GetAsync(string url);
    Task<PharmacyUrl> GetTestAsync(Guid pharmacyId, Guid userId);
}
