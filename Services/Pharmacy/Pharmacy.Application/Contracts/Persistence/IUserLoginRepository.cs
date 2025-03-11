using Pharmacy.Domain;

namespace Pharmacy.Application.Contracts.Persistence
{
    public interface IUserLoginRepository
    {
        Task<Guid> CreateAsync(UserLogin userLogin);

        Task<bool> UpdateAsync(UserLogin userLogin);

        Task<bool> DeleteAsync(UserLogin userLogin);

        Task<IEnumerable<UserLogin>> GetAllAsync();

        Task<UserLogin> GetByIdAsync(Guid id);
    }
}
