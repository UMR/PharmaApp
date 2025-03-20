namespace Pharmacy.Application.Features.Customer.Dtos;

public class CustomerRegDto
{
    public Guid Id { get; set; }

    public Guid PharmacyId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public short Age { get; set; }

    public float Weight { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
