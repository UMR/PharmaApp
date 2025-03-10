using Pharmacy.Domain;

namespace Pharmacy.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<User> GetAsync(string loginId, string pin);
        Task<byte?> IsActiveAsync(Guid id);
        Task<User> GetByIdAsync(Guid id);
        Task<bool> IsExistAsync(string loginId);
        Task<Guid> CreateAsync(User user);
    }
}
