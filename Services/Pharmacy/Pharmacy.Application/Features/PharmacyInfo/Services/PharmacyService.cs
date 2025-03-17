using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Exceptions;
using Pharmacy.Application.Features.CurrentUser.Services;
using Pharmacy.Application.Features.PharmacyInfo.Dtos;
using Pharmacy.Application.Features.PharmacyInfo.Validators;
using Pharmacy.Application.Features.PharmacyUrls.Services;
using QRCoder;

namespace Pharmacy.Application.Features.PharmacyInfo.Services
{
    public class PharmacyService : IPharmacyService
    {
        #region Fields

        private readonly ICurrentUserService _currentUserService;
        private readonly IPharmacyRepository _pharmacyRepository;
        private readonly IConfiguration _configuration;
        private readonly IPharmacyUrlService _pharmacyUrlService;
        
        #endregion

        #region Ctro

        public PharmacyService(ICurrentUserService currentUserService, 
            IPharmacyRepository pharmacyRepository,
            IConfiguration configuration,
            IPharmacyUrlService pharmacyUrlService) 
        { 
            _currentUserService = currentUserService;
            _pharmacyRepository = pharmacyRepository;
            _configuration = configuration;
            _pharmacyUrlService = pharmacyUrlService;
        }

        #endregion

        #region Methods

        public async Task UpdateAsync(PharmacyUpdateDto request)
        {
            var validator = new PharmacyUpdateDtoValidator();
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

            if (pharmacy == null)
            {
                pharmacy = new Pharmacy.Domain.Pharmacy();
                pharmacy.OwnerId = _currentUserService.UserId;               
                pharmacy.StoreName = request.StoreName;
                pharmacy.AddressLine1 = request.AddressLine1;
                pharmacy.AddressLine2 = request.AddressLine2;

                pharmacy.CreatedBy = _currentUserService.UserId;
                pharmacy.CreatedDate = DateTime.UtcNow;

                pharmacy.StoreLogo = string.IsNullOrEmpty(filename) ? pharmacy.StoreLogo : filename;

                await _pharmacyRepository.AddAsync(pharmacy);
            }
            else 
            {
                pharmacy.StoreName = request.StoreName;
                pharmacy.AddressLine1 = request.AddressLine1;
                pharmacy.AddressLine2 = request.AddressLine2;

                pharmacy.UpdatedDate = DateTime.UtcNow;
                pharmacy.UpdatedBy = _currentUserService.UserId;

                pharmacy.StoreLogo = string.IsNullOrEmpty(filename) ? pharmacy.StoreLogo : filename;

                await _pharmacyRepository.UpdateAsync(pharmacy);
            }
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
            string filename = $"{DateTime.Now.Ticks.ToString()}_{storeLogo.FileName}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images/Logo", filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await storeLogo.CopyToAsync(stream);
            }

            return filename;
        }

        #endregion
    }
}
