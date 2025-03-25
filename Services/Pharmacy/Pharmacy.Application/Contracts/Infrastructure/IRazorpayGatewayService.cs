namespace Pharmacy.Application.Contracts.Infrastructure;

public interface IRazorpayGatewayService
{
    string CreateOrder(decimal amount, string currency);

    bool VerifyPayment(string orderId, string paymentId, string signature);

    object GetKey();

    object GetPaymentDetails(string paymentId);
}
