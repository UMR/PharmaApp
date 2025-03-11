using Pharmacy.Application.Features.User.Dtos;

namespace Pharmacy.Application.Features.CurrentUser.Services
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }

        string UserIpAddress { get; }

        UserInfoDto User { get; }
    }
}
