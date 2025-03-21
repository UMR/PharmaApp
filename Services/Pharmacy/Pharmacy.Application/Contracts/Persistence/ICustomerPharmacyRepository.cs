using Pharmacy.Application.Features.CustomerPharmacy.Dtos;
using Pharmacy.Application.Wrapper;
using Pharmacy.Domain;

namespace Pharmacy.Application.Contracts.Persistence;

public interface ICustomerPharmacyRepository
{
    ValueTask<bool> CreateAsync(CustomerPharmacy customerPharmacy);
    Task<PaginatedList<PharmacyUserScanHistoryDto>> GetScanHistoryByIdAsync(Guid pharmacyId, int pageIndex, int pageSize);
}
