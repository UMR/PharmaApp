using Pharmacy.Application.Features.PharmacyInfo.Dtos;

namespace Pharmacy.Application.Features.PharmacyInfo.Services
{
    public interface IPharmacyService
    {
        Task<PharmacyDto> GetAsync();
        Task UpdateAsync(PharmacyUpdateDto request);
        ValueTask<string> GenerateQRCodeAsync();
    }
}
