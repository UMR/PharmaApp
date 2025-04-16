using System.Web;
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
    private readonly ITokenGenerationService _tokenGenerationService;

    #endregion

    #region Ctor

    public PaymentController(IPaymentService paymentService, IRazorpayGatewayService razorpayGatewayService, ITokenGenerationService tokenGenerationService)
    {
        _paymentService = paymentService;
        _razorpayGatewayService = razorpayGatewayService;
        _tokenGenerationService = tokenGenerationService;
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

    [HttpPost("Create")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePaymentDto paymentInfoDto)
    {
        var isVerified = _razorpayGatewayService.VerifyPayment(paymentInfoDto.OrderId, paymentInfoDto.PaymentId, paymentInfoDto.Signature);

        if (!isVerified)
        {
            return BadRequest("Payment verification failed.");
        }

        var result = await _paymentService.CreatePaymentAsync(paymentInfoDto);

        if (!result)
        {
            return StatusCode(500, "Unable to create payment.");
        }

        string token = await _tokenGenerationService.GenerateTokenAsync(paymentInfoDto.PharmacyId, paymentInfoDto.CustomerId);

        return Ok( new
        {
            token = token
        });
    }

    [HttpGet("getkey")]
    [AllowAnonymous]
    public IActionResult GetKeyAsync()
    {
        return Ok(_razorpayGatewayService.GetKey());
    }

    [HttpGet("VerityToken/{pharmacyId}/{customerId}")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyTokenAsync([FromRoute] Guid pharmacyId, [FromRoute] Guid customerId, [FromQuery] string token)
    {
        var decodedToken = HttpUtility.UrlDecode(token);
        
        bool isValid = await _tokenGenerationService.VerifyTokenAsync(decodedToken, pharmacyId, customerId);

        if (isValid)
        {
            return Ok();
        }

        return StatusCode(500, "Invalid Token");
    }

    #endregion
}
