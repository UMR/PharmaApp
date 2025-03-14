
using Newtonsoft.Json.Linq;
using Pharmacy.Domain.Enums;
using System.Security.Claims;

namespace Pharmacy.Api.Policy
{
    public class ActivePharmacyHandler : AuthorizationHandler<ActivePharmacyRequirement>
    {
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
            }

            return Task.CompletedTask;
        }
    }
}
