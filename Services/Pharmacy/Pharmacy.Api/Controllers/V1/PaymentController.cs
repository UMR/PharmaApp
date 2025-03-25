using Pharmacy.Application.Contracts.Infrastructure;
using Pharmacy.Application.Features.Payment.Services;

namespace Pharmacy.Api.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    #region Fields

    private readonly IPaymentService _paymentService;
    private readonly IRazorpayGatewayService _razorpayGatewayService;

    #endregion

    #region Ctor

    public PaymentController(IPaymentService paymentService, IRazorpayGatewayService razorpayGatewayService)
    {
        _paymentService = paymentService;
        _razorpayGatewayService = razorpayGatewayService;
    }

    #endregion

    #region Methods

    [HttpGet("Create/Order")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateOrderAsync(Guid packageId, string currencyCode)
    {
        var orderId = _paymentService.CreateOrderAsync(packageId, currencyCode);

        return Ok(orderId);
    }

    [HttpGet("getkey")]
    [AllowAnonymous]
    public IActionResult GetKeyAsync()
    {
        return Ok(_razorpayGatewayService.GetKey());
    }

    #endregion
}
