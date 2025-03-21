using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Features.CustomerPharmacy.Dtos;
using Pharmacy.Application.Wrapper;
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

    public async Task<PaginatedList<PharmacyUserScanHistoryDto>> GetScanHistoryByIdAsync(Guid pharmacyId, int pageIndex, int pageSize)
    {
        var customerPharmacies = _context.CustomerPharmacies.AsNoTracking()
            .Where(cusP => cusP.PharmacyId == pharmacyId).Include(cusP => cusP.Customer);

        var totalRow = await customerPharmacies.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRow / (double)pageSize);


        var scanHistory = await customerPharmacies.OrderByDescending(cusP => cusP.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize)
            .Select(cusP => new PharmacyUserScanHistoryDto
            {
                PharmacyId = cusP.PharmacyId,
                CustomerId = cusP.CustomerId,
                CustomerFirstName = cusP.Customer.FirstName,
                CustomerLastName = cusP.Customer.LastName ?? "",
                CustomerEmail = cusP.Customer.Email ?? "",
                CustomerMobile = cusP.Customer.Mobile ?? "",
                CustomerAge = cusP.Customer.Age,
                CustomerWeight = (float)cusP.Customer.Weight,
                ScanDate = cusP.CreatedDate,
            }).ToListAsync();

        var result = new PaginatedList<PharmacyUserScanHistoryDto>(scanHistory, totalRow, totalPages, pageIndex);

        return result;
    }

    #endregion
}
