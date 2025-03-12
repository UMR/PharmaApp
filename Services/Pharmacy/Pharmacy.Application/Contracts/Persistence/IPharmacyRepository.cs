namespace Pharmacy.Application.Contracts.Persistence
{
    public interface IPharmacyRepository
    {
        Task<Domain.Pharmacy> GetPharmacyByUserIdAsync(Guid userId);
        Task<bool> AddAsync(Pharmacy.Domain.Pharmacy pharmacy);
        Task<bool> UpdateAsync(Pharmacy.Domain.Pharmacy pharmacy);
    }
}
