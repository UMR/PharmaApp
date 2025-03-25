namespace Pharmacy.Application.Features.PackageFeature.Dtos;

public class PackageDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string CurrencyCode { get; set; }
    public decimal CommissionInPercent { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
}
