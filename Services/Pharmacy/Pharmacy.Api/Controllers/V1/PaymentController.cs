using Pharmacy.Application.Contracts.Infrastructure;
using Pharmacy.Application.Features.Payment.Dtos;
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

    [HttpGet("Create/Order/{packageId}/{currencyCode}")]
    [AllowAnonymous]
    public IActionResult CreateOrder([FromRoute] Guid packageId, [FromRoute] string currencyCode)
    {
        var orderId = _paymentService.CreateOrder(packageId, currencyCode);

        return Ok(orderId);
    }

    [HttpGet("Create")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePaymentDto paymentInfoDto)
    {
        var isVerified = _razorpayGatewayService.VerifyPayment(paymentInfoDto.OrderId, paymentInfoDto.PaymentId, paymentInfoDto.Signature);

        if (isVerified)
        {
            return BadRequest("Payment verification failed.");
        }

        var result = await _paymentService.CreatePaymentAsync(paymentInfoDto);

        if (result)
        {
            return StatusCode(500, "Unable to create payment.");
        }

        return Ok();
    }

    [HttpGet("getkey")]
    [AllowAnonymous]
    public IActionResult GetKeyAsync()
    {
        return Ok(_razorpayGatewayService.GetKey());
    }

    #endregion
}
