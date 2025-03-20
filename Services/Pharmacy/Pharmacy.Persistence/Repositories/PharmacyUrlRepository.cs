using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Domain;

namespace Pharmacy.Persistence.Repositories;

public class PharmacyUrlRepository : IPharmacyUrlRepository
{
    #region Fields

    private readonly PharmaAppDbContext _context;

    #endregion

    #region Ctro

    public PharmacyUrlRepository(PharmaAppDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<bool> AddAsync(PharmacyUrl pharmacyUrl)
    {
        await _context.PharmacyUrls.AddAsync(pharmacyUrl);
        int result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<PharmacyUrl?> GetAsync(Guid pharmacyId)
    {
        var pharmacyUniqueUlr = await _context.PharmacyUrls.AsNoTracking().Where(pu => pu.PharmacyId == pharmacyId).Include(pu => pu.Pharmacy).FirstOrDefaultAsync();

        return pharmacyUniqueUlr;
    }

    public async Task<PharmacyUrl?> GetAsync(string url)
    {
        var pharmacyUniqueUlr = await _context.PharmacyUrls.AsNoTracking().Where(pu => pu.Url == url).Include(pu => pu.Pharmacy).FirstOrDefaultAsync();

        return pharmacyUniqueUlr;
    }

    #endregion
}
