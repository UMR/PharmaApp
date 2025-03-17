using Pharmacy.Domain;

namespace Pharmacy.Application.Features.PharmacyUrls.Services;

public interface IPharmacyUrlService
{
    Task<PharmacyUrl> GetAsync(Guid pharmacyId);
}
