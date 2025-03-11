using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Application.Features.Authentication.Dtos;
using Pharmacy.Application.Features.Authentication.Services;

namespace Pharmacy.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields

        private readonly IAuthenticationService _authenticationService;

        #endregion

        #region Ctor

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        #endregion

        #region Methods

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(UserRegisterRequestDto request)
        {
            return Ok(await _authenticationService.RegisterAsync(request));
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(UserLoginRequestDto request)
        {
            return Ok(await _authenticationService.LoginAsync(request));
        }

        #endregion
    }
}
