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
        [HttpGet("Get")]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _pharmacyService.GetAsync();

            return Ok(result);
        }


        [Authorize(Policy = RoleConstant.Pharmacist)]
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] PharmacyUpdateDto request)
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

            return Ok(new {
                base64String = imageBase64String
            });
        }

        #endregion
    }
}
