using Pharmacy.Application.Features.Payment.Dtos;

namespace Pharmacy.Application.Features.Payment.Services;

public interface IPaymentService
{
    object CreateOrder(Guid packageId, string currencyCode);
    ValueTask<bool> CreatePaymentAsync(CreatePaymentDto paymentInfoDto);
}
