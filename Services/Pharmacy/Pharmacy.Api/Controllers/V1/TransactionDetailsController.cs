using Newtonsoft.Json;
using Pharmacy.Application.Features.TransactionDetails.Dtos;
using Pharmacy.Application.Features.TransactionDetails.Services;

namespace Pharmacy.Api.Controllers.V1;

[Route("api/v1/[controller]")]
[ApiController]
public class TransactionDetailsController : ControllerBase
{
    #region Fields

    private readonly ITransactionDetailsService _transactionDetailsService;

    #endregion

    #region Ctro

    public TransactionDetailsController(ITransactionDetailsService transactionDetailsService)
    {
        _transactionDetailsService = transactionDetailsService;
    }

    #endregion
    
    #region Methods
    
    [HttpGet("daily/{fromDate}/{toDate}")]
    [Authorize(Policy = RoleConstant.Pharmacist)]
    public async Task<IActionResult> GetDailyTranDetailsAsync([FromRoute] DateTimeOffset fromDate, [FromRoute] DateTimeOffset toDate, [FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string? filters)
    {
        var filterRequest = JsonConvert.DeserializeObject<TransactionDetailsFiltersDto>(filters ?? "") ?? new TransactionDetailsFiltersDto();
        var result = await _transactionDetailsService.GetDailyTransactionDetailsAsync(fromDate, toDate, pageIndex, pageSize, filterRequest);

        return Ok(result);
    }

    [HttpGet("monthly/{fromDate}/{toDate}")]
    [Authorize(Policy = RoleConstant.Pharmacist)]
    public async Task<IActionResult> GetMonthlyTranDetailsAsync([FromRoute] DateTimeOffset fromDate, [FromRoute] DateTimeOffset toDate, [FromQuery] int pageIndex, [FromQuery] int pageSize, [FromQuery] string? filters)
    {
        var filterRequest = JsonConvert.DeserializeObject<TransactionDetailsFiltersDto>(filters ?? "") ?? new TransactionDetailsFiltersDto();
        var result = await _transactionDetailsService.GetMonthlyTransactionDetailsAsync(fromDate, toDate, pageIndex, pageSize, filterRequest);

        return Ok(result);
    }

    #endregion
}

