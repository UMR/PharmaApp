using Pharmacy.Application.Features.Authentication.Dtos;
using Pharmacy.Application.Models;

namespace Pharmacy.Application.Features.Authentication.Services
{
    public interface IAuthenticationService
    {
        Task<TokenResponseDto> RegisterAsync(UserRegisterRequestDto request);

        Task<TokenResponseDto> LoginAsync(UserLoginRequestDto request);
    }
}
