namespace Pharmacy.Api.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class PharmacyController : ControllerBase
{
    #region Fields

    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IPharmacyService _pharmacyService;
    private readonly IPharmacyUrlService _pharmacyUrlService;
    private readonly ICustomerPharmacyService _customerPharmacyService;

    #endregion

    #region Ctor

    public PharmacyController(IHttpContextAccessor httpContextAccessor,
                            IPharmacyService pharmacyService, 
                            IPharmacyUrlService pharmacyUrlService,
                            ICustomerPharmacyService customerPharmacyService)
    {
        _contextAccessor = httpContextAccessor;
        _pharmacyService = pharmacyService;
        _pharmacyUrlService = pharmacyUrlService;
        _customerPharmacyService = customerPharmacyService;
    }

    #endregion

    #region Methods

    [Authorize(Policy = RoleConstant.Pharmacist)]
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var result = await _pharmacyService.GetAsync();

        if(result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }

    [Authorize(Policy = RoleConstant.Admin)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var result = await _pharmacyService.GetAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("Url/{url}")]
    public async Task<IActionResult> GetAsync([FromRoute] string url)
    {
        var result = await _pharmacyUrlService.GetAsync(url);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [Authorize(Policy = RoleConstant.Pharmacist)]
    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync([FromForm] PharmacyDto request)
    {
        var pharmacy = await _pharmacyService.CreateAsync(request);

        if(pharmacy == null)
        {
            return StatusCode(500, new { message = "Unable to create pharmacy. Please try later." });
        }

        string url = $"{_contextAccessor?.HttpContext?.Request.Scheme}://{_contextAccessor?.HttpContext?.Request.Host}api/v1/pharmacy/{pharmacy.Id}";

        return Created(url, pharmacy);
    }

    [Authorize(Policy = RoleConstant.Pharmacist)]
    [HttpPut("Update")]
    public async Task<IActionResult> UpdateAsync([FromForm] PharmacyDto request)
    {
        var pharmacy = await _pharmacyService.UpdateAsync(request);
        
        if(pharmacy == null)
        {
            return StatusCode(500, new { message = "Unable to update pharmacy. Please try later." });
        }

        return Ok(pharmacy);
    }

    [Authorize(Policy = $"Active{RoleConstant.Pharmacist}")]
    [HttpGet("QRCode")]
    public async Task<IActionResult> GenerateQRCode()
    {
        string imageBase64String = await _pharmacyService.GenerateQRCodeAsync();

        if (imageBase64String.IsNullOrEmpty())
        {
            return StatusCode(500, new { message = "Unable to generate QR code. Please try later." });
        }

        return Ok(new {
            base64String = imageBase64String
        });
    }

    [HttpGet("ScanHistory/{pharmacyId}")]
    [Authorize(Policy = $"Active{RoleConstant.Pharmacist}")]
    public async Task<IActionResult> GetScanHistory(Guid pharmacyId, int pageIndex, int pageSize)
    {
        var result = await _customerPharmacyService.GetScanHistoryByIdAsync(pharmacyId, pageIndex, pageSize);
        
        return Ok(result);
    }

    #endregion
}
