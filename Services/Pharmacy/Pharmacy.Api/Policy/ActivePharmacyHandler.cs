
using Newtonsoft.Json.Linq;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Features.CurrentUser.Services;
using Pharmacy.Application.Features.User.Services;
using Pharmacy.Domain.Enums;
using System.Security.Claims;

namespace Pharmacy.Api.Policy
{
    public class ActivePharmacyHandler : AuthorizationHandler<ActivePharmacyRequirement>
    {
        private readonly IUserService _userService;

        public ActivePharmacyHandler(IUserService userService)
        {
            _userService = userService;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActivePharmacyRequirement requirement)
        {
            var user = context.User.FindFirst(c => c.Type == "user");

            if(user != null)
            {
                JObject userObject = JObject.Parse(user.Value);
                
                Enum.TryParse(userObject.GetValue("status").ToString(), true, out UserStatusEnum status);

                if (status == UserStatusEnum.Active)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    Guid.TryParse(userObject.GetValue("id").ToString(), out Guid userId);

                    var task = Task.Run(async () =>
                    {
                        return await _userService.GetByIdAsync(userId);
                    }, CancellationToken.None);

                    task.Wait();

                    var userInfo = task.Result;

                    if(userInfo != null && userInfo.Status.Equals((byte)1))
                    {
                        context.Succeed(requirement);
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
