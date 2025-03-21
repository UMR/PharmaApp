using Pharmacy.Application.Features.CustomerPharmacy.Dtos;
using Pharmacy.Application.Wrapper;

namespace Pharmacy.Application.Features.CustomerPharmacy.Services;

public interface ICustomerPharmacyService
{
    Task<Domain.CustomerPharmacy?> CreateAsync(Guid customerId, Guid pharmacyId);
    Task<PaginatedList<PharmacyUserScanHistoryDto>> GetScanHistoryByIdAsync(Guid pharmacyId, int pageIndex, int pageSize);
}
