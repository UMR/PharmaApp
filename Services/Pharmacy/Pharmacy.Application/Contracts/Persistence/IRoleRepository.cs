using Pharmacy.Domain;

namespace Pharmacy.Application.Contracts.Persistence
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync();

        Task<Role> GetByIdAsync(Guid id);

        Task<Role> GetByNameAsync(string name);

        Task<List<string>> GetRolesByUserIdAsync(Guid id);

        Task<List<string>> GetRolesByUserAsync(User user);
    }
}
