using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Domain;

namespace Pharmacy.Persistence.Repositories;

public class CustomerPharmacyRepository : ICustomerPharmacyRepository
{
    #region Fields

    private readonly PharmaAppDbContext _context;

    #endregion

    #region Ctro

    public CustomerPharmacyRepository(PharmaAppDbContext context)
    { 
        _context = context;
    }    
    
    #endregion

    #region Methods

    public async ValueTask<bool> CreateAsync(CustomerPharmacy customerPharmacy)
    {
        await _context.CustomerPharmacies.AddAsync(customerPharmacy);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    #endregion
}
