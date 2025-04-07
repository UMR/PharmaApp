using System;
using System.Collections.Generic;

namespace Pharmacy.Domain;

public partial class PaymentDetail
{
    public Guid Id { get; set; }

    public Guid PharmacyId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid PackageId { get; set; }

    public decimal PackagePrice { get; set; }

    public decimal PackageCommissionInPercent { get; set; }

    public DateTimeOffset PackageLastUpdatedOn { get; set; }

    public Guid PackageLastUpdatedBy { get; set; }

    public string OrderId { get; set; } = null!;

    public string PaymentId { get; set; } = null!;

    public string Signature { get; set; } = null!;

    public string PaymentLog { get; set; } = null!;

    public decimal PaidAmount { get; set; }

    public decimal? Discount { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Package Package { get; set; } = null!;

    public virtual Pharmacy Pharmacy { get; set; } = null!;
}
