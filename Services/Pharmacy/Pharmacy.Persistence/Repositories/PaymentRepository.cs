﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Features.PharmacyInfo.Dtos;
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

    public async Task<PaginatedList<TransactionDetailsResponseDto>> GetDailyPaymentDetailsAsync(Guid pharmacyId, DateTimeOffset utcFromDate, DateTimeOffset utcToDate, int pageIndex, int pageSize, TransactionDetailsFiltersDto filters)
    {
        var paymentDetails = _context.PaymentDetails.AsNoTracking().Where(p =>
            (p.CreatedDate).Date >= utcFromDate.Date && p.CreatedDate.Date <= utcToDate.Date);

        IOrderedQueryable<PaymentDetail> orderedPaymentDetails = null;

        if (filters.CreatedDate.Equals("ASC", StringComparison.OrdinalIgnoreCase))
        {
            orderedPaymentDetails = paymentDetails.OrderBy(p => p.CreatedDate);
        }
        else
        {
            orderedPaymentDetails = paymentDetails.OrderByDescending(p => p.CreatedDate);
        }
       
      
        var response = await GetTransactionDetailsResponse(orderedPaymentDetails.Include(p => p.Package), pageIndex, pageSize);

        return response;
    }

    public async Task<PaginatedList<TransactionDetailsResponseDto>> GetMonthlyPaymentDetailsAsync(Guid pharmacyId, DateTimeOffset utcFromDate, DateTimeOffset utcToDate, int pageIndex, int pageSize, TransactionDetailsFiltersDto filters)
    {
        var paymentDetails = _context.PaymentDetails.AsNoTracking().Where(p =>
            (
                (p.CreatedDate).Month >= utcFromDate.Month && p.CreatedDate.Month <= utcToDate.Month) && 
                (p.CreatedDate.Year >= utcFromDate.Year && p.CreatedDate.Year <= utcToDate.Year)
            );
        
        IOrderedQueryable<PaymentDetail> orderedPaymentDetails = null;

        if (filters.CreatedDate.Equals("ASC", StringComparison.OrdinalIgnoreCase))
        {
            orderedPaymentDetails = paymentDetails.OrderBy(p => p.CreatedDate);
        }
        else
        {
            orderedPaymentDetails = paymentDetails.OrderByDescending(p => p.CreatedDate);
        }

        var response = await GetTransactionDetailsResponse(orderedPaymentDetails.Include(p => p.Package), pageIndex, pageSize);
        
        return response;
    }

    #endregion

    #region Private

    private async Task<PaginatedList<TransactionDetailsResponseDto>> GetTransactionDetailsResponse(IIncludableQueryable<PaymentDetail, Package> paymentDetails, int pageIndex, int pageSize)
    {
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
