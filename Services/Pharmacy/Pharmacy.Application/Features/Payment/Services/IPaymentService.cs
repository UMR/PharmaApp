namespace Pharmacy.Application.Features.Payment.Services;

public interface IPaymentService
{
    object CreateOrderAsync(Guid packageId, string currencyCode);
}
