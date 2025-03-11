using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Domain;

namespace Pharmacy.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PharmaAppDbContext _context;

        public UserRepository(PharmaAppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetAsync(string loginId, string pin)
        {
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => (u.Email.ToUpper() == loginId.Trim().ToUpper() || u.Mobile == loginId.Trim())
                && u.Pin == pin.Trim());
            return user;
        }

        public async Task<byte?> IsActiveAsync(Guid id)
        {
            var status = await _context.Users
                               .AsNoTracking()
                               .Where(u => u.Id == id)
                               .Select(u => u.Status)
                               .FirstOrDefaultAsync();

            return status;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<bool> IsExistAsync(string loginId)
        {
            var result = await _context.Users.AsNoTracking()
                .AnyAsync(u => u.Email.ToUpper() == loginId.Trim().ToUpper() || u.Mobile == loginId.Trim());
            return result;
        }

        public async Task<Guid> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> IsExistEmailAsync(Guid id, string email)
        {
            var result = await _context.Users.AsNoTracking().AnyAsync(u => u.Id != id && u.Email.ToUpper() == email.Trim().ToUpper());
            return result;
        }

        public async Task<bool> IsExistMobileAsync(Guid id, string mobile)
        {
            var result = await _context.Users.AsNoTracking().AnyAsync(u => u.Id != id && u.Mobile == mobile.Trim());
            return result;
        }
    }
}