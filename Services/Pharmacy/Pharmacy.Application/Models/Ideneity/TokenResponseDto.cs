﻿namespace Pharmacy.Application.Models;

public class TokenResponseDto
{

    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("errorDescription ")]
    public string ErrorDescription { get; set; }

}
