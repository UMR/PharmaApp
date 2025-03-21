﻿using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Domain;

namespace Pharmacy.Persistence.Repositories
{
    public class PharmacyRepository: IPharmacyRepository
    {
        #region Fields

        private readonly PharmaAppDbContext _context;

        #endregion

        #region Ctor

        public PharmacyRepository(PharmaAppDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<Domain.Pharmacy> GetPharmacyByUserIdAsync(Guid userId)
        {
            var result = await _context.Pharmacies.Where(p => p.OwnerId == userId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> AddAsync(Pharmacy.Domain.Pharmacy pharmacy)
        {           
            await _context.Pharmacies.AddAsync(pharmacy);

            int result = await _context.SaveChangesAsync();

            return (result > 0) ? true : false;
        }

        public async Task<bool> UpdateAsync(Pharmacy.Domain.Pharmacy pharmacy)
        {
            _context.Pharmacies.Update(pharmacy);

            int result = await _context.SaveChangesAsync();

            return (result > 0) ? true: false; 
        }

        #endregion
    }
}
