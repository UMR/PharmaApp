using Pharmacy.Application.Models.SMS;

namespace Pharmacy.Application.Contracts.Infrastructure
{
    public interface ISMSService
    {
        Task<string> SendSMSAsync(SMS sms);
        Task SendErrorSMSAsync(Exception message);
    }
}
