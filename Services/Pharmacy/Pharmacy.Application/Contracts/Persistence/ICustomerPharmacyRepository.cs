using Pharmacy.Domain;

namespace Pharmacy.Application.Contracts.Persistence;

public interface ICustomerPharmacyRepository
{
    ValueTask<bool> CreateAsync(CustomerPharmacy customerPharmacy);
}
