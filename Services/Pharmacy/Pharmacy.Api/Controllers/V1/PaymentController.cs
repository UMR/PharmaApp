using Pharmacy.Application.Features.Payment.Services;

namespace Pharmacy.Api.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    #region Fields

    private readonly IPaymentService _paymentService;

    #endregion

    #region Ctor

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    #endregion

    #region Methods

    [HttpGet("Create/Order")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateOrderAsync(Guid packageId, string currencyCode)
    {
        var orderId = _paymentService.CreateOrderAsync(packageId, currencyCode);

        return Ok(new
        {
            orderId = orderId
        });
    }


    #endregion
}
