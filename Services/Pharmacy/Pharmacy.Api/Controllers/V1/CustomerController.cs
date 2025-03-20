using Pharmacy.Application.Features.Customer.Dtos;
using Pharmacy.Application.Features.Customer.Services;

namespace Pharmacy.Api.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    #region Fields

    private readonly ICustomerService _customerService;

    #endregion

    #region Ctor

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    #endregion

    #region Methods

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync([FromBody] CustomerRegDto customerInfo)
    {
        await _customerService.RegisterAsync(customerInfo);

        return Created();
    }

    #endregion
}
