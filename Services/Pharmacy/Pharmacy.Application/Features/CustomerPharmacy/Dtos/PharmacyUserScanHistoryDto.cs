namespace Pharmacy.Application.Features.CustomerPharmacy.Dtos;

public class PharmacyUserScanHistoryDto
{
    public Guid PharmacyId { get; set; }

    public Guid CustomerId { get; set; }

    public string CustomerFirstName { get; set; } = string.Empty;

    public string CustomerLastName { get; set; } = string.Empty;

    public string CustomerMobile { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public int CustomerAge { get; set; } = 0;

    public float CustomerWeight { get; set; } = 0;

    public DateTime ScanDate { get; set; }
}
