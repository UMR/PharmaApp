using Pharmacy.Application.Contracts.Persistence;

namespace Pharmacy.Application.Features.CustomerPharmacy.Services;

public class CustomerPharmacyService : ICustomerPharmacyService
{
    #region Fields

    private readonly ICustomerPharmacyRepository _customerPharmacyRepository;

    #endregion

    #region Ctro

    public CustomerPharmacyService(ICustomerPharmacyRepository customerPharmacyRepository)
    {
        _customerPharmacyRepository = customerPharmacyRepository;
    }

    #endregion

    #region Methods

    public async Task<Domain.CustomerPharmacy?> CreateAsync(Guid customerId, Guid pharmacyId)
    {
        var customerPharmacy = new Domain.CustomerPharmacy();

        customerPharmacy.Id = Guid.NewGuid();
        customerPharmacy.CustomerId = customerId;
        customerPharmacy.PharmacyId = pharmacyId;
        customerPharmacy.CreatedBy = customerId;
        customerPharmacy.CreatedDate = DateTime.UtcNow;

        var result = await _customerPharmacyRepository.CreateAsync(customerPharmacy);
        
        return (result) ? customerPharmacy : null;
    }
    #endregion
}
