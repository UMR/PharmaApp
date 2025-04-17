using Pharmacy.Application.Features.User.Dtos;
using Pharmacy.Application.Wrapper;
using Pharmacy.Domain.Enums;

namespace Pharmacy.Application.Features.User.Services
{
    public interface IUserService
    {
        Task<UserInfoDto> GetUserAsync(string loginId, string pin);
        Task<bool> IsActiveAsync(Guid id);
        Task<UserInfoDto> GetByIdAsync(Guid id);
        Task UpdateAsync(Guid id, UserUpdateDto request);
        ValueTask<bool> IsExistAsync(string loginId);
        Task<bool> UpdateUserStatusAsync(Guid id, string status);
        Task<bool> IsUserExistsAsync(UserRegisterRequestDto userRegisterRequestDto);
        Task<PaginatedList<UserInfoDto>> GetAllUserAsync(string? search, int pageNumber, int pageSize);


    }
}
