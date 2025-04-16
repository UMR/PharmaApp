using Pharmacy.Application.Features.TransactionDetails.Dtos;
using Pharmacy.Application.Wrapper;

namespace Pharmacy.Application.Features.TransactionDetails.Services;

public class TransactionDetailsService: ITransactionDetailsService
{
    #region Fields
    
    private readonly IPaymentRepository _paymentRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPharmacyService _pharmacyService;

    #endregion

    #region Ctro

    public TransactionDetailsService(IPaymentRepository paymentRepository, 
                    ICurrentUserService currentUserService, 
                    IPharmacyService pharmacyService)
    {
        _paymentRepository = paymentRepository;
        _currentUserService = currentUserService;
        _pharmacyService = pharmacyService;
    }

    #endregion

    #region Methods

    public async Task<PaginatedList<TransactionDetailsResponseDto>> GetDailyTransactionDetailsAsync(DateTimeOffset fromDate, DateTimeOffset toDate, int pageIndex, int pageSize, TransactionDetailsFiltersDto filters)
    {
        if (pageIndex <= 0) pageIndex = 0;
        if (pageSize <= 0) pageSize = 10;
        if (fromDate == DateTimeOffset.MinValue) fromDate = DateTimeOffset.UtcNow;
        if(toDate == DateTimeOffset.MinValue) toDate = DateTimeOffset.UtcNow;

        var pharmayc = await _pharmacyService.GetAsync();
        var result = await _paymentRepository.GetDailyPaymentDetailsAsync(pharmayc.Id, fromDate.ToUniversalTime(), toDate.ToUniversalTime(), pageIndex, pageSize, filters);

        return result;
    }

    public async Task<PaginatedList<TransactionDetailsResponseDto>> GetMonthlyTransactionDetailsAsync(DateTimeOffset fromDate, DateTimeOffset toDate, int pageIndex, int pageSize, TransactionDetailsFiltersDto filters)
    {
        if (pageIndex <= 0) pageIndex = 0;
        if (pageSize <= 0) pageSize = 10;
        if (fromDate == DateTimeOffset.MinValue) fromDate = DateTimeOffset.UtcNow;
        if(toDate == DateTimeOffset.MinValue) toDate = DateTimeOffset.UtcNow;

        var pharmayc = await _pharmacyService.GetAsync();
        var result = await _paymentRepository.GetMonthlyPaymentDetailsAsync(pharmayc.Id, fromDate.ToUniversalTime(), toDate.ToUniversalTime(), pageIndex, pageSize, filters);

        return result;
    }

    #endregion
}

