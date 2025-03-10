using Pharmacy.Application.Features.User.Dtos;

namespace Pharmacy.Application.Features.User.Services
{
    public interface IUserService
    {
        Task<UserInfoDto> GetUserAsync(string loginId, string pin);
        Task<bool> IsActiveAsync(Guid id);
        Task<UserInfoDto> GetByIdAsync(Guid id);
    }
}
