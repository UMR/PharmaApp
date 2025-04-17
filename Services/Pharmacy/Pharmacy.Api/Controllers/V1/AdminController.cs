using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pharmacy.Application.Features.User.Services;

namespace Pharmacy.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Policy = RoleConstant.Admin)]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] string search = "", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var users = await _userService.GetAllUserAsync(search, page, pageSize);
            return Ok(users);
        }

        [Authorize(Policy = RoleConstant.Admin)]
        [HttpPut("UpdateUserStatus/{id:guid}/{status}")]
        public async Task<IActionResult> UpdateUserStatusAsync(Guid id, string status)
        {
            var result = await _userService.UpdateUserStatusAsync(id, status);
            return Ok(result);
        }

    }
}