namespace Pharmacy.Application.Features.Payment.Dtos;

public class CreatePaymentDto
{
    //public Guid Id { get; set; }
    public Guid CustomerId { get; set;}
    public Guid PharmacyId { get; set; }
    public Guid PackageId { get; set; }
    public string OrderId {  get; set; }
    public string PaymentId { get; set; }
    public string Signature { get; set; }

}
