using Pharmacy.Application.Contracts.Infrastructure;
using Pharmacy.Application.Features.PackageFeature.Dtos;
using Pharmacy.Application.Features.PackageFeature.Services;
using Pharmacy.Application.Features.Payment.Dtos;
using Pharmacy.Domain;

namespace Pharmacy.Application.Features.Payment.Services;

public class PaymentService : IPaymentService
{
    #region Fields

    private readonly IRazorpayGatewayService _razorpayGatewayService;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPackageService _packageService;

    #endregion

    #region Ctro

    public PaymentService(IRazorpayGatewayService razorpayGatewayService,
        IPaymentRepository paymentRepository, 
        IPackageService packageService)
    {
        _razorpayGatewayService = razorpayGatewayService;
        _paymentRepository = paymentRepository;
        _packageService = packageService;

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
        var package = await _packageService.GetAsync(paymentInfoDto.PackageId);

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
