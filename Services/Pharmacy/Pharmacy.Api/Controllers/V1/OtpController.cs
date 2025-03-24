using Pharmacy.Application.Common.Enums;
using Pharmacy.Application.Contracts.Infrastructure;
using Pharmacy.Application.Models;

namespace Pharmacy.Api.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class OtpController : ControllerBase
{
    private readonly IOtpService _otpService;

    public OtpController(IOtpService otpService)
    {
        _otpService = otpService;
    }

    [AllowAnonymous]
    [HttpPost("generate/{login}/{type?}")]
    public async Task<IActionResult> GenerateOtp(string login, OtpType? type = null)
    {
        return Ok(await _otpService.GenerateOtp(login, type));
    }

    [AllowAnonymous]
    [HttpPost("verify")]
    public IActionResult VerifyOtp([FromBody] OtpVerificationRequest request)
    {
        return Ok(_otpService.VerifyOtp(request.LoginId, request.Otp, request.Type));
    }
}
