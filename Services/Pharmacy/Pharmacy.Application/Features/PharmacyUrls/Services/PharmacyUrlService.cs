using AutoMapper;
using Pharmacy.Application.Contracts.Infrastructure;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Features.CurrentUser.Services;
using Pharmacy.Application.Features.PharmacyInfo.Dtos;
using Pharmacy.Domain;
using System.Text;

namespace Pharmacy.Application.Features.PharmacyUrls.Services;

public class PharmacyUrlService: IPharmacyUrlService
{
    #region Fields
    
    private readonly IPharmacyUrlRepository _pharmacyUrlRepository;
    private readonly IUniqueIdService _uniqueIdService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private const string base62String = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const int baseValue = 62;

    #endregion

    #region Ctro

    public PharmacyUrlService(IPharmacyUrlRepository pharmacyUrlRepository, 
        IUniqueIdService uniqueIdService, 
        ICurrentUserService currentUserService, 
        IMapper mapper)
    {
        _pharmacyUrlRepository = pharmacyUrlRepository;
        _uniqueIdService = uniqueIdService;
        _currentUserService = currentUserService;
        _mapper = mapper;
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

    public async Task<PharmacyDetailsDto?> GetAsync(string url)
    {
        var pharmacyUrl = await _pharmacyUrlRepository.GetAsync(url);
        var result = _mapper.Map<PharmacyDetailsDto>(pharmacyUrl?.Pharmacy);

        return result;
    }

    #endregion

    #region Private Methods

    private string GetBase62String(long id)
    {
        StringBuilder sb = new StringBuilder();

        try 
        {
            while (id > 0)
            {
                int remender = (int)(id % baseValue);
                id = id / baseValue;
                sb.Append(base62String[remender]);
            }

        }
        catch (Exception ex){
            Console.WriteLine(ex);
        }
        
        return sb.ToString();
    }

    #endregion
}
