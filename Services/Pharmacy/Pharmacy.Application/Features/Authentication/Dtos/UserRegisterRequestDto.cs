using Microsoft.AspNetCore.Http;

namespace Pharmacy.Application.Features.Authentication.Dtos
{
    public class UserRegisterRequestDto
    {

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Mobile { get; set; } = string.Empty;

        public string Pin { get; set; } = string.Empty;
    }
}
