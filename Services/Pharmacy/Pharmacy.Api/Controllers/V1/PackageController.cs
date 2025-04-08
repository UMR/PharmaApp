using Pharmacy.Application.Features.PackageFeature.Services;

namespace Pharmacy.Api.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class PackageController : ControllerBase
{
    #region Fields

    private readonly IPackageService _packageService;

    #endregion
    
    #region Ctro

    public PackageController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    #endregion

    #region Methods

    [HttpGet("latest")]
    [AllowAnonymous]
    public async Task<IActionResult> GetLatestPackage()
    {
        var result = _packageService.GetLatestAsync();

        return Ok(result);
    }

    //[HttpPut("/update")]
    //[AllowAnonymous]
    //public async Task<IActionResult> UpdatePackage()
    //{
    //    return null;
    //}

    #endregion
}
