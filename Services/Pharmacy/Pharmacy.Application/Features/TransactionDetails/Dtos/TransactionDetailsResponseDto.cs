namespace Pharmacy.Application.Features.TransactionDetails.Dtos;

public class TransactionDetailsResponseDto
{
    public Guid Id { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public string PackageCurrency { get; set; } = string.Empty;
    public decimal PackagePrice { get; set; } = 0;
    public decimal PackageCommission { get; set; } = 0;
    public decimal Profit { get; set; } = 0;
    public DateTimeOffset CreatedDate { get; set; }
    public decimal TotalProfit { get; set; } = 0;
    public decimal TotalPackagePrice { get; set; } = 0;
}
