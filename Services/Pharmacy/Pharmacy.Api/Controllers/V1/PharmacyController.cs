using Pharmacy.Application.Common.Constants;
using Pharmacy.Application.Features.PharmacyInfo.Dtos;
using Pharmacy.Application.Features.PharmacyInfo.Services;

namespace Pharmacy.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PharmacyController : ControllerBase
    {
        #region Fields

        private readonly IPharmacyService _pharmacyService;

        #endregion

        #region Ctor

        public PharmacyController(IPharmacyService pharmacyService)
        {
            _pharmacyService = pharmacyService;
        }

        #endregion

        #region Methods

        [Authorize(Policy = $"Active{RoleConstant.Pharmacist}")]
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync(PharmacyUpdateDto request)
        {
            await _pharmacyService.UpdateAsync(request);

            // we will pass the url and object in the created method
            return Created();
        }

        [Authorize(Policy = $"Active{RoleConstant.Pharmacist}")]
        [HttpGet("GetQRCode")]
        public async Task<IActionResult> GenerateQRCode()
        {
            string imageBase64String = _pharmacyService.GenerateQRCode();

            return Ok(imageBase64String);
        }

        #endregion
    }
}
