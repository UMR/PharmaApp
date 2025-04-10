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
    public async Task<IActionResult> GetDailyTranDetailsAsync(DateTimeOffset fromDate, DateTimeOffset toDate, int pageIndex, int pageSize)
    {
        var result = await _transactionDetailsService.GetDailyTransactionDetailsAsync(fromDate, toDate, pageIndex, pageSize);

        return Ok(result);
    }

    [HttpGet("monthly/{fromDate}/{toDate}")]
    [Authorize(Policy = RoleConstant.Pharmacist)]
    public async Task<IActionResult> GetMonthlyTranDetailsAsync(DateTimeOffset fromDate, DateTimeOffset toDate, int pageIndex, int pageSize)
    {
        var result = await _transactionDetailsService.GetMonthlyTransactionDetailsAsync(fromDate, toDate, pageIndex, pageSize);

        return Ok(result);
    }

    #endregion
}

