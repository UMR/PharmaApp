using Pharmacy.Application.Features.PackageFeature.Dtos;

namespace Pharmacy.Api.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class PackageController : ControllerBase
{
    #region Fields
    
    #endregion
    
    #region Ctro

    public PackageController()
    {

    }

    #endregion

    #region Methods

    [HttpGet("latest")]
    [AllowAnonymous]
    public async Task<IActionResult> GetLatestPackage()
    {
        var package = new PackageDto();
        package.Id = Guid.Parse("91dd28d1-980c-4031-897d-9215c7954eed");
        package.Name = "Single scan package";
        package.Description = "There will be only 1 scan under this package.";
        package.Price = 99.00M;
        package.CurrencyCode = "INR";
        package.CommissionInPercent = 15.34;
        package.CreatedDate = DateTime.Parse("3/25/2025 6:10:40 AM");
        package.CreatedBy = Guid.Parse("b6970dae-1d97-4884-be10-56a0c5088f0b");

        return Ok(package);
    }

    //[HttpPut("/update")]
    //[AllowAnonymous]
    //public async Task<IActionResult> UpdatePackage()
    //{
    //    return null;
    //}

    #endregion
}
