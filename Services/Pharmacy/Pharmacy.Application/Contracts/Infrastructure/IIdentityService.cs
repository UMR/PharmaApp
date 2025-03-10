using Pharmacy.Application.Models;

namespace Pharmacy.Application.Contracts.Infrastructure
{
    public interface IIdentityService
    {
        Task<TokenResponseDto> GetToken(string loginId, string pin);

        Task<TokenResponseDto> GetRefreshToken(string refreshToken);

        Task RevokeRefreshToken(string refreshToken);
    }
}
