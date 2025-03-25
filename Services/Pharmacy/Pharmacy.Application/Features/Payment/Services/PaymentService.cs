using MailKit.Search;
using Pharmacy.Application.Contracts.Infrastructure;

namespace Pharmacy.Application.Features.Payment.Services;

public class PaymentService : IPaymentService
{
    #region Fields

    private readonly IRazorpayGatewayService _razorpayGatewayService;

    #endregion

    #region Ctro

    public PaymentService(IRazorpayGatewayService razorpayGatewayService)
    {
        _razorpayGatewayService = razorpayGatewayService;
    }

    #endregion

    #region Methods

    public object CreateOrderAsync(Guid packageId, string currencyCode)
    {        
        var orderId = _razorpayGatewayService.CreateOrder(99, currencyCode);

        return (new
        {
            orderId = orderId
        });
    }

    #endregion
}
