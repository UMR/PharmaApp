using Pharmacy.Application.Features.PharmacyInfo.Dtos;
using Pharmacy.Application.Features.PharmacyInfo.Validators;
using Pharmacy.Application.Features.PharmacyUrls.Services;
using QRCoder;

namespace Pharmacy.Application.Features.PharmacyInfo.Services;

public class PharmacyService : IPharmacyService
{
    #region Fields

    private readonly ICurrentUserService _currentUserService;
    private readonly IPharmacyRepository _pharmacyRepository;
    private readonly IConfiguration _configuration;
    private readonly IPharmacyUrlService _pharmacyUrlService;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;

    #endregion

    #region Ctro

    public PharmacyService(ICurrentUserService currentUserService,
        IPharmacyRepository pharmacyRepository,
        IConfiguration configuration,
        IPharmacyUrlService pharmacyUrlService,
        IMapper mapper,
        IServiceProvider serviceProvider) 
    { 
        _currentUserService = currentUserService;
        _pharmacyRepository = pharmacyRepository;
        _configuration = configuration;
        _pharmacyUrlService = pharmacyUrlService;
        _mapper = mapper;
        _serviceProvider = serviceProvider;
    }

    #endregion

    #region Methods

    public async Task<PharmacyDetailsDto> GetAsync()
    {
        var pharmacy = await _pharmacyRepository.GetPharmacyByUserIdAsync(_currentUserService.UserId);

        var result = _mapper.Map<PharmacyDetailsDto>(pharmacy);
        
        return result;
    }

    public async Task<PharmacyDetailsDto> GetAsync(Guid pharmacyId)
    {
        var pharmacy = await _pharmacyRepository.GetByIdAsync(pharmacyId);

        var result = _mapper.Map<PharmacyDetailsDto>(pharmacy);

        return result;
    }

    public async Task<PharmacyDetailsDto?> CreateAsync(PharmacyDto request)
    {
        var validator = new PharmacyDtoValidator(_serviceProvider);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsValid == false)
        {
            throw new ValidationRequestException(validationResult.Errors);
        }

        var storedPharmacy = await _pharmacyRepository.GetPharmacyByUserIdAsync(_currentUserService.UserId);

        if(storedPharmacy != null)
        {
            throw new Exception("A pharmacy is available under the current user. Please update exist pharmacy instead.");
        }

        string filename = "";

        if (request.StoreLogo != null && request.StoreLogo.Length > 0)
        {
            filename = await UploadStoreLogoAsync(request.StoreLogo);
        }

        var pharmacy = new Domain.Pharmacy();
        pharmacy.OwnerId = _currentUserService.UserId;
        pharmacy.StoreName = request.StoreName.Trim();
        pharmacy.AddressLine1 = request.AddressLine1.Trim();
        pharmacy.AddressLine2 = request.AddressLine2?.Trim();

        pharmacy.CreatedBy = _currentUserService.UserId;
        pharmacy.CreatedDate = DateTime.UtcNow;

        pharmacy.StoreLogo = string.IsNullOrEmpty(filename) ? pharmacy.StoreLogo : filename;

        var response = await _pharmacyRepository.AddAsync(pharmacy);

        if(response)
        {
            return _mapper.Map<PharmacyDetailsDto>(pharmacy);
        }

        return null;          
    }

    public async Task<PharmacyDetailsDto?> UpdateAsync(PharmacyDto request)
    {
        var validator = new PharmacyDtoValidator(_serviceProvider);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.IsValid == false)
        {
            throw new ValidationRequestException(validationResult.Errors);
        }

        var pharmacy = await _pharmacyRepository.GetPharmacyByUserIdAsync(_currentUserService.UserId);
        string filename = "";

        if (request.StoreLogo != null && request.StoreLogo.Length > 0)
        {
            filename = await UploadStoreLogoAsync(request.StoreLogo);
        }

        pharmacy.StoreName = request.StoreName.Trim();
        pharmacy.AddressLine1 = request.AddressLine1.Trim();
        pharmacy.AddressLine2 = request.AddressLine2.Trim();

        pharmacy.UpdatedDate = DateTime.UtcNow;
        pharmacy.UpdatedBy = _currentUserService.UserId;

        pharmacy.StoreLogo = string.IsNullOrEmpty(filename) ? pharmacy.StoreLogo : filename;

        var response = await _pharmacyRepository.UpdateAsync(pharmacy);

        if (response)
        {
            return _mapper.Map<PharmacyDetailsDto>(pharmacy);
        }

        return null;
    }

    public async ValueTask<string> GenerateQRCodeAsync()
    {
        string baseURL = _configuration.GetValue<string>("BaseURL");
        var pharmacy = await _pharmacyRepository.GetPharmacyByUserIdAsync(_currentUserService.UserId);

        var pharmacyUrl = await _pharmacyUrlService.GetAsync(pharmacy.Id);

        string qrCodeImageString = string.Empty;

        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        {
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{baseURL}/scan?pharmacy={pharmacyUrl.Url}", QRCodeGenerator.ECCLevel.H))
            {
                using (Base64QRCode qrCode = new Base64QRCode(qrCodeData))
                {
                    qrCodeImageString = qrCode.GetGraphic(20);
                }
            }
        }

        return qrCodeImageString;
    }

    #endregion

    #region Private Methods

    private async ValueTask<string> UploadStoreLogoAsync(IFormFile storeLogo)
    {
        string directory = Path.Combine(Directory.GetCurrentDirectory(), "Images/Logo");

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string filename = $"{DateTime.Now.Ticks.ToString()}_{storeLogo.FileName}";
        var filePath = Path.Combine(directory, filename);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await storeLogo.CopyToAsync(stream);
        }

        return filename;
    }

    #endregion
}
