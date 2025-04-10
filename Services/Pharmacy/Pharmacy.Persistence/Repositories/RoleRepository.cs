using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Domain;

namespace Pharmacy.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly PharmaAppDbContext _context;
        
        public RoleRepository(PharmaAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            var roles = await _context.Roles.AsNoTracking().ToListAsync();
            return roles;
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            return role;
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            var role = await _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Name.ToUpper() == name.Trim().ToUpper());
            return role;
        }

        public async Task<List<string>> GetRolesByUserAsync(User user)
        {
            var userRoles = await _context.UserRoles.AsNoTracking()
            .Where(ur => ur.UserId == user.Id)
            .Select(ur => ur.Role.Name)
            .ToListAsync();

            return userRoles;
        }

        public async Task<List<string>> GetRolesByUserIdAsync(Guid id)
        {
            var userRoles = await _context.UserRoles.AsNoTracking()
            .Where(ur => ur.UserId == id)
            .Select(ur => ur.Role.Name)
            .ToListAsync();

            return userRoles;
        }
    }
}
