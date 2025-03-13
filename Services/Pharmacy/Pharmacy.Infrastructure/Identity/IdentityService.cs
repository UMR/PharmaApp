namespace Pharmacy.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    #region Fields

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _address;
    private readonly string _identityClientId = "dhanvantariweb";
    private readonly string _identityClientSecret = "s*|9%2~*=95*+|t8*~3**%;U73*+-c";
    private readonly string _identityScope = "dhanvantari.fullaccess offline_access";

    #endregion

    #region Ctro

    public IdentityService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _address = _configuration.GetSection("IdentityServer")["ValidIssuer"];           
    }

    #endregion

    #region Methods

    public async Task<TokenResponseDto> GetToken(string loginId, string pin)
    {
        var response = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            Address = $"{_address}/connect/token",
            GrantType = "password",
            ClientId = _identityClientId,
            ClientSecret = _identityClientSecret,
            Scope = _identityScope,
            UserName = loginId,
            Password = pin
        });

        if (!response.IsError)
        {
            return JsonConvert.DeserializeObject<TokenResponseDto>(response.Raw);
        }
        else
        {
            return new TokenResponseDto { Error = "Invalid login", ErrorDescription = response.ErrorDescription };
        }
    }

    public async Task<TokenResponseDto> GetRefreshToken(string refreshToken)
    {
        var response = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
        {
            Address = $"{_address}/connect/token",
            GrantType = "refresh_token",
            RefreshToken = refreshToken,
            ClientId = _identityClientId,
            ClientSecret = _identityClientSecret
        });

        if (!response.IsError)
        {
            return JsonConvert.DeserializeObject<TokenResponseDto>(response.Raw);
        }
        else
        {
            return new TokenResponseDto { Error = "Invalid refresh token", ErrorDescription = response.ErrorDescription };
        }
    }

    public async Task RevokeRefreshToken(string token)
    {
        var discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(_address);
        if (discoveryDocument.IsError)
        {
            return;
        }

        var response = await _httpClient.RevokeTokenAsync(new TokenRevocationRequest
        {
            Address = discoveryDocument.RevocationEndpoint,
            ClientId = _identityClientId,
            ClientSecret = _identityClientSecret,
            Token = token
        });

        if (response.IsError)
        {
            //throw new BadRequestException(response.Error);
        }
        else
        {
            var responseString = response.Raw;
        }
    }

    #endregion
}
