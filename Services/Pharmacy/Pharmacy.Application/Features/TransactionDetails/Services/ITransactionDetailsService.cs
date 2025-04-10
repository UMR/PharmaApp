using Pharmacy.Application.Features.TransactionDetails.Dtos;
using Pharmacy.Application.Wrapper;

namespace Pharmacy.Application.Features.TransactionDetails.Services;

public interface ITransactionDetailsService
{

    Task<PaginatedList<TransactionDetailsResponseDto>> GetDailyTransactionDetailsAsync(DateTimeOffset fromDate,
        DateTimeOffset toDate, int pageIndex, int pageSize);

    Task<PaginatedList<TransactionDetailsResponseDto>> GetMonthlyTransactionDetailsAsync(DateTimeOffset fromDate,
        DateTimeOffset toDate, int pageIndex, int pageSize);
}

