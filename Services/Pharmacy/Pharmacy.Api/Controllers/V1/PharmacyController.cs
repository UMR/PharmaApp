using Pharmacy.Application.Common.Constants;
using Pharmacy.Application.Features.PharmacyInfo.Dtos;
using Pharmacy.Application.Features.PharmacyInfo.Services;
using Pharmacy.Application.Features.PharmacyUrls.Services;

namespace Pharmacy.Api.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PharmacyController : ControllerBase
    {
        #region Fields

        private readonly IPharmacyService _pharmacyService;
        private readonly IPharmacyUrlService _pharmacyUrlService;

        #endregion

        #region Ctor

        public PharmacyController(IPharmacyService pharmacyService, IPharmacyUrlService pharmacyUrlService)
        {
            _pharmacyService = pharmacyService;
            _pharmacyUrlService = pharmacyUrlService;
        }

        #endregion

        #region Methods

        [Authorize(Policy = RoleConstant.Pharmacist)]
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
            string imageBase64String = await _pharmacyService.GenerateQRCodeAsync();

            return Ok(imageBase64String);
        }

        [HttpGet("GetPharmacyUrlTest")]
        [AllowAnonymous]
        public async Task<IActionResult> GeneratePharmacyUrlTest(string pharmacyId, string userId)
        {
            Guid userGuid = Guid.Parse(userId);
            Guid pharmacyGuid = Guid.Parse(pharmacyId);
            
            var result = await _pharmacyUrlService.GetTestAsync(pharmacyGuid, userGuid);

            return Ok(result);
        }

        #endregion
    }
}
