namespace Pharmacy.Application.Features.CustomerPharmacy.Services;

public interface ICustomerPharmacyService
{
    Task<Domain.CustomerPharmacy?> CreateAsync(Guid customerId, Guid pharmacyId);
}
