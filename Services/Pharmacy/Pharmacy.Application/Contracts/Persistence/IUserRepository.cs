using Pharmacy.Domain;

namespace Pharmacy.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<User> GetAsync(string loginId, string pin);
        Task<User> GetAsync(string loginId);
        Task<byte?> IsActiveAsync(Guid id);
        Task<User> GetByIdAsync(Guid id);
        Task<bool> IsExistAsync(string loginId);
        Task<Guid> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> IsExistEmailAsync(Guid id, string email);
        Task<bool> IsExistMobileAsync(Guid id, string mobile);
    }
}
