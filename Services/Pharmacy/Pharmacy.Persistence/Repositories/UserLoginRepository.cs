using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Domain;

namespace Pharmacy.Persistence.Repositories
{
    public class UserLoginRepository : IUserLoginRepository
    {
        #region Fields

        private readonly PharmaAppDbContext _context;

        #endregion

        #region Ctro

        public UserLoginRepository(PharmaAppDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<Guid> CreateAsync(UserLogin userLocation)
        {
            await _context.UserLogins.AddAsync(userLocation);
            await _context.SaveChangesAsync();
            return userLocation.Id;
        }
        

        public async Task<bool> UpdateAsync(UserLogin userLocation)
        {
            _context.UserLogins.Update(userLocation);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<bool> DeleteAsync(UserLogin userLocation)
        {
            _context.UserLogins.Remove(userLocation);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }

        public async Task<IEnumerable<UserLogin>> GetAllAsync()
        {
            var userLocations = await _context.UserLogins.AsNoTracking().ToListAsync();
            return userLocations;
        }

        public async Task<UserLogin> GetByIdAsync(Guid id)
        {
            var userLocations = await _context.UserLogins.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            return userLocations;
        }

        #endregion
    }
}
