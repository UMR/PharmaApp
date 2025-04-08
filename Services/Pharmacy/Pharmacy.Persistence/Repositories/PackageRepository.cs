using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Domain;

namespace Pharmacy.Persistence.Repositories;

public class PackageRepository: IPackageRepository
{
    #region Fields
    
    private readonly PharmaAppDbContext _context;

    #endregion

    #region Ctro

    public PackageRepository(PharmaAppDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods
    
    public async Task<Package>GetAsync()
    {
        var result = await _context.Packages.OrderByDescending(p => p.CreatedDate).FirstOrDefaultAsync();

        return result;
    }

    public async Task<Package> GetAsync(Guid packageId)
    {
        var result = await _context.Packages.Where(p => p.Id == packageId).FirstOrDefaultAsync();

        return result;
    }

    #endregion
}
