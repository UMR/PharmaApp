using Pharmacy.Application.Common.Constants;
using Pharmacy.Application.Features.CurrentUser.Services;
using Pharmacy.Application.Features.User.Dtos;
using Pharmacy.Application.Features.User.Services;

namespace Pharmacy.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;

        #endregion

        #region Ctor

        public UserController(IUserService userService, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _currentUserService = currentUserService;
        }

        #endregion

        #region Methods

        [Authorize(Policy = RoleConstant.Pharmacist)]
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UserUpdateDto request)
        {
            await _userService.UpdateAsync(id, request);
            return NoContent();
        }

        [Authorize(Policy = RoleConstant.Pharmacist)]
        [HttpGet("Get")]
        public async Task<IActionResult> GetAsync()
        {
            var userInfo = _currentUserService.User;
            return Ok(userInfo);
        }

        #endregion
    }
}