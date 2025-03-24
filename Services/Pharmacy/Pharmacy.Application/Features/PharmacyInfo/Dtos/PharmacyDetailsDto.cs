namespace Pharmacy.Application.Features.PharmacyInfo.Dtos;
public class PharmacyDetailsDto
{
    public Guid Id { get; set; }

    public Guid OwnerId { get; set; }

    public string StoreName { get; set; } = null!;

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string? StoreLogo { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
