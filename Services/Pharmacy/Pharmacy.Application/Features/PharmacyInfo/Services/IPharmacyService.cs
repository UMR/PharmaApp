using Pharmacy.Application.Features.PharmacyInfo.Dtos;

namespace Pharmacy.Application.Features.PharmacyInfo.Services
{
    public interface IPharmacyService
    {
        Task UpdateAsync(PharmacyUpdateDto request);
    }
}
