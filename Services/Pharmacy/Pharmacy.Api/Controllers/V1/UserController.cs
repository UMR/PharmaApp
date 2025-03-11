using Pharmacy.Application.Common.Constants;
using Pharmacy.Application.Features.User.Dtos;
using Pharmacy.Application.Features.User.Services;

namespace Pharmacy.Api.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Fields

        private readonly IUserService _userService;

        #endregion

        #region Ctor

        public UserController(IUserService userService) 
        { 
            _userService = userService;
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

        #endregion
    }
}