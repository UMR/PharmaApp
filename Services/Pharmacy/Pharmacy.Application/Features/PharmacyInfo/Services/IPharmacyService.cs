using Pharmacy.Application.Features.PharmacyInfo.Dtos;

namespace Pharmacy.Application.Features.PharmacyInfo.Services
{
    public interface IPharmacyService
    {
        Task<PharmacyDetailsDto> GetAsync();
        Task<PharmacyDetailsDto> GetAsync(Guid pharmacyId);
        Task<PharmacyDetailsDto?> CreateAsync(PharmacyDto request);
        Task<PharmacyDetailsDto?> UpdateAsync(PharmacyDto request);
        ValueTask<string> GenerateQRCodeAsync();
    }
}
