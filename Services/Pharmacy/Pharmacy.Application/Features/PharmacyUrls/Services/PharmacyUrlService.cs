using Pharmacy.Application.Contracts.Infrastructure;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Features.CurrentUser.Services;
using Pharmacy.Domain;
using System.Text;

namespace Pharmacy.Application.Features.PharmacyUrls.Services;

public class PharmacyUrlService: IPharmacyUrlService
{
    #region Fields
    
    private readonly IPharmacyUrlRepository _pharmacyUrlRepository;
    private readonly IUniqueIdService _uniqueIdService;
    private readonly ICurrentUserService _currentUserService;
    private const string base62String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const int baseValue = 62;

    #endregion

    #region Ctro

    public PharmacyUrlService(IPharmacyUrlRepository pharmacyUrlRepository, 
        IUniqueIdService uniqueIdService, 
        ICurrentUserService currentUserService)
    {
        _pharmacyUrlRepository = pharmacyUrlRepository;
        _uniqueIdService = uniqueIdService;
        _currentUserService = currentUserService;
    }

    #endregion

    #region Public Methods

    public async Task<PharmacyUrl> GetAsync(Guid pharmacyId)
    {
        var pharmacyUrl = await _pharmacyUrlRepository.GetAsync(pharmacyId);
        
        if(pharmacyUrl == null)
        {
            long uniqueId = _uniqueIdService.GetNextID();

            pharmacyUrl = new PharmacyUrl();
            pharmacyUrl.PharmacyId = pharmacyId;
            pharmacyUrl.Id = uniqueId;
            pharmacyUrl.Url = GetBase62String(uniqueId);
            pharmacyUrl.CreatedBy = _currentUserService.UserId;

            await _pharmacyUrlRepository.AddAsync(pharmacyUrl);
        }

        return pharmacyUrl;
    }

    #endregion

    #region Private Methods

    private string GetBase62String(long id)
    {
        StringBuilder sb = new StringBuilder();

        while (id > 0)
        {
            int remender = (int) (id % baseValue);
            id = id / baseValue;

            sb.Append(base62String[remender]);
        }

        return sb.ToString();
    }

    #endregion
}
