using Pharmacy.Application.Models.Email;

namespace Pharmacy.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task SendEmailAsync(Email email);
        Task SendErrorEmailAsync(Exception message);
    }
}
