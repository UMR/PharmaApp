using Microsoft.AspNetCore.Http;

namespace Pharmacy.Application.Features.PharmacyInfo.Dtos
{
    public class PharmacyUpdateDto
    {
        public Guid Id { get; set; }
        public string StoreName {get; set;} = string.Empty;
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public IFormFile? StoreLogo { get; set;}
    }
}
