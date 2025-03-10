namespace Pharmacy.Application.Models;

public class RefreshTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; }
}
