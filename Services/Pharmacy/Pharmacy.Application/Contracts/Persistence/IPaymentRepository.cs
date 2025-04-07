using Pharmacy.Domain;

namespace Pharmacy.Application.Contracts.Persistence;

public interface IPaymentRepository
{
    ValueTask<bool> CreatePaymentAsync(PaymentDetail paymentDetail);
}
