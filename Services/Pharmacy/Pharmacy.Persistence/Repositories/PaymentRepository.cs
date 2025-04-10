using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Features.TransactionDetails.Dtos;
using Pharmacy.Application.Wrapper;
using Pharmacy.Domain;

namespace Pharmacy.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    #region Fields

    private readonly PharmaAppDbContext _context;

    #endregion

    #region Ctor

    public PaymentRepository(PharmaAppDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async ValueTask<bool> CreatePaymentAsync(PaymentDetail paymentDetail)
    {
        await _context.PaymentDetails.AddAsync(paymentDetail);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<PaginatedList<TransactionDetailsResponseDto>> GetDailyPaymentDetailsAsync(Guid pharmacyId, DateTimeOffset utcFromDate, DateTimeOffset utcToDate, int pageIndex, int pageSize)
    {
        var paymentDetails = _context.PaymentDetails.Where(p =>
            (p.CreatedDate).Date >= utcFromDate.Date && p.CreatedDate.Date <= utcToDate)
            .Include(p => p.Package);

        var totalProfit = await paymentDetails.SumAsync(p => (p.PackagePrice * p.PackageCommissionInPercent) / 100);
        var totalPackagePrice = await paymentDetails.SumAsync(p => p.PackagePrice);
        var totalScan = await paymentDetails.CountAsync();
        var totalPages = (int)Math.Ceiling(totalScan / (double)pageSize);

        var result = await paymentDetails.Skip((pageIndex - 1) * pageSize).Take(pageSize)
            .Select(p => new TransactionDetailsResponseDto()
            {
                Id = p.Id,
                PackageName = p.Package.Name,
                PackageCurrency = p.Package.CurrencyCode,
                PackagePrice = p.PackagePrice,
                PackageCommission = p.PackageCommissionInPercent,
                Profit = (p.PackagePrice * p.PackageCommissionInPercent)/100,
                CreatedDate = p.CreatedDate,
                TotalProfit = totalProfit,
                TotalPackagePrice = totalPackagePrice
            }).ToListAsync();

        var response  = new PaginatedList<TransactionDetailsResponseDto>(result, totalScan, totalPages, pageIndex);

        return response;
    }

    #endregion
}
