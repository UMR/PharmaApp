namespace Pharmacy.Application.Contracts.Persistence
{
    public interface IPharmacyRepository
    {
        ValueTask<bool> IsPharmacyExistAsync(Guid pharmacyId);
        Task<Domain.Pharmacy?> GetByIdAsync(Guid pharmacyId);
        Task<Domain.Pharmacy> GetPharmacyByUserIdAsync(Guid userId);
        Task<bool> AddAsync(Pharmacy.Domain.Pharmacy pharmacy);
        Task<bool> UpdateAsync(Pharmacy.Domain.Pharmacy pharmacy);
    }
}
