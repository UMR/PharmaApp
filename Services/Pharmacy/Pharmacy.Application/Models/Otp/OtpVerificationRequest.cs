using  Pharmacy.Application.Common.Enums;

namespace Pharmacy.Application.Models;

public class OtpVerificationRequest
{
    public string LoginId { get; set; }

    public string Otp { get; set; }

    public OtpType? Type { get; set; } = null;
}
