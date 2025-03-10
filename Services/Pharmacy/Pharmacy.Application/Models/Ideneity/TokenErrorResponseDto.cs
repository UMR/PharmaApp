﻿namespace Pharmacy.Application.Models;

public class TokenErrorResponseDto
{
    [JsonProperty("errorDescription")]
    public string ErrorDescription { get; set; }

    [JsonProperty("isError")]
    public bool IsError { get; set; }

    [JsonProperty("httpErrorReason")]
    public string HttpErrorReason { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }
}
