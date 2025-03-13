namespace Pharmacy.Infrastructure.Identity;

public class ProfileService: IProfileService
{
    private readonly ILogger<ProfileService> _logger;
    private readonly IUserService _userService;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserLoginService _userLocationService;

    public ProfileService(ILogger<ProfileService> logger, IUserService userService, IRoleRepository roleRepository, IUserLoginService userLocationService)
    {
        _logger = logger;
        _userService = userService;
        _roleRepository = roleRepository;
        _userLocationService = userLocationService;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var claims = new ClaimsIdentity();

        try
        {
            var user = await _userService.GetByIdAsync(new Guid(context.Subject.GetSubjectId()));
            if (user != null)
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                };
                var currentUserJson = JsonConvert.SerializeObject(user, jsonSettings);

                var newClaims = new List<Claim>()
            {
                new Claim("user", currentUserJson)
            };

                var roles = await _roleRepository.GetRolesByUserIdAsync(user.Id);
                if (roles != null && roles.Count > 0)
                {
                    foreach (var role in roles)
                    {
                        newClaims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

                claims.AddClaims(newClaims);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        context.IssuedClaims = claims.Claims.ToList();
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        try
        {
            context.IsActive = await _userService.IsActiveAsync(new Guid(context.Subject.GetSubjectId()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
