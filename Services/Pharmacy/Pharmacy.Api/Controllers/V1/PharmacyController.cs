using Pharmacy.Application.Common.Constants;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Features.CustomerPharmacy.Services;
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
        private readonly ICustomerPharmacyService _customerPharmacyService;

        #endregion

        #region Ctor

        public PharmacyController(IPharmacyService pharmacyService, 
                                IPharmacyUrlService pharmacyUrlService,
                                ICustomerPharmacyService customerPharmacyService)
        {
            _pharmacyService = pharmacyService;
            _pharmacyUrlService = pharmacyUrlService;
            _customerPharmacyService = customerPharmacyService;
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

        [AllowAnonymous]
        [HttpGet("GetByUrl/{url}")]
        public async Task<IActionResult> GetAsync(string url)
        {
            var result = await _pharmacyUrlService.GetAsync(url);

            return (result == null) ? NotFound() : Ok(result);
        }

        [Authorize(Policy = RoleConstant.Pharmacist)]
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync([FromForm] PharmacyUpdateDto request)
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

        [HttpGet("GetScanHistory/{pharmacyId}")]
        [Authorize(Policy = $"Active{RoleConstant.Pharmacist}")]
        public async Task<IActionResult> GetScanHistory(Guid pharmacyId, int pageIndex, int pageSize)
        {
            var result = await _customerPharmacyService.GetScanHistoryByIdAsync(pharmacyId, pageIndex, pageSize);
            
            return Ok(result);
        }

        #endregion
    }
}
