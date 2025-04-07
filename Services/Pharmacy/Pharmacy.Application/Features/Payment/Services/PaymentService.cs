using Pharmacy.Application.Contracts.Infrastructure;
using Pharmacy.Application.Features.PackageFeature.Dtos;
using Pharmacy.Application.Features.Payment.Dtos;
using Pharmacy.Domain;

namespace Pharmacy.Application.Features.Payment.Services;

public class PaymentService : IPaymentService
{
    #region Fields

    private readonly IRazorpayGatewayService _razorpayGatewayService;
    private readonly IPaymentRepository _paymentRepository;

    #endregion

    #region Ctro

    public PaymentService(IRazorpayGatewayService razorpayGatewayService, IPaymentRepository paymentRepository)
    {
        _razorpayGatewayService = razorpayGatewayService;
        _paymentRepository = paymentRepository;
    }

    #endregion

    #region Methods

    public object CreateOrder(Guid packageId, string currencyCode)
    {
        var orderId = _razorpayGatewayService.CreateOrder(99, currencyCode);

        return (new
        {
            orderId = orderId
        });
    }

    public async ValueTask<bool> CreatePaymentAsync(CreatePaymentDto paymentInfoDto)
    {
        // we will fetch this information from database.
        var package = new PackageDto();
        // package.Id = Guid.Parse("91dd28d1-980c-4031-897d-9215c7954eed");
        package.Name = "Single scan package";
        package.Description = "There will be only 1 scan under this package.";
        package.Price = 99.00M;
        package.CurrencyCode = "INR";
        package.CommissionInPercent = 15.34M;
        package.CreatedDate = DateTime.Parse("3/25/2025 6:10:40 AM");
        package.CreatedBy = Guid.Parse("b6970dae-1d97-4884-be10-56a0c5088f0b");

        var paymentLog = _razorpayGatewayService.GetPaymentDetails(paymentInfoDto.PaymentId);

        var paymentDetails = new PaymentDetail()
        {
            Id = Guid.NewGuid(),
            PharmacyId = paymentInfoDto.PharmacyId,
            CustomerId = paymentInfoDto.CustomerId,
            PackageId = paymentInfoDto.PackageId,
            PackagePrice = package.Price,
            PackageCommissionInPercent = package.CommissionInPercent,
            PackageLastUpdatedOn = DateTime.UtcNow,
            PackageLastUpdatedBy = Guid.NewGuid(),
            OrderId = paymentInfoDto.OrderId,
            PaymentId = paymentInfoDto.PaymentId,
            Signature = paymentInfoDto.Signature,
            PaymentLog = paymentLog.ToString(),
            PaidAmount = package.Price,
            CreatedBy = Guid.NewGuid(),
            CreatedDate = DateTime.UtcNow
        };

        var result = await _paymentRepository.CreatePaymentAsync(paymentDetails);

        return result;
    }

    #endregion
}
