using Pharmacy.Application.Features.TransactionDetails.Dtos;
using Pharmacy.Application.Wrapper;
using Pharmacy.Domain;

namespace Pharmacy.Application.Contracts.Persistence;

public interface IPaymentRepository
{
    ValueTask<bool> CreatePaymentAsync(PaymentDetail paymentDetail);

    Task<PaginatedList<TransactionDetailsResponseDto>> GetDailyPaymentDetailsAsync(Guid pharmacyId,
        DateTimeOffset utcFromDate, DateTimeOffset utcToDate, int pageIndex, int pageSize, TransactionDetailsFiltersDto filters);

    Task<PaginatedList<TransactionDetailsResponseDto>> GetMonthlyPaymentDetailsAsync(Guid pharmacyId,
        DateTimeOffset utcFromDate, DateTimeOffset utcToDate, int pageIndex, int pageSize, TransactionDetailsFiltersDto filters);
}
