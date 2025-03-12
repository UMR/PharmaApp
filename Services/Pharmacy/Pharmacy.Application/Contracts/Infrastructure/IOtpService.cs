using Pharmacy.Application.Common.Enums;
using Pharmacy.Application.Models;
using Pharmacy.Application.Wrapper;

namespace Pharmacy.Application.Contracts.Infrastructure
{
    public interface IOtpService
    {
        Task<BaseQueryResponse<OtpResponse>> GenerateOtp(string loginId, OtpType? type);
        BaseQueryResponse<String> VerifyOtp(string loginId, string otp, OtpType? type);
    }
}
