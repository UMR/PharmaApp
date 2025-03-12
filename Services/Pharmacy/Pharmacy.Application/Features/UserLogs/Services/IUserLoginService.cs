namespace Pharmacy.Application.Features.UserLogs.Services
{
    public interface IUserLoginService
    {
        Task CreateAsync(Guid userId);
    }
}
