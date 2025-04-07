using System;
using System.Collections.Generic;

namespace Pharmacy.Domain;

public partial class Package
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public decimal CommissionInPercent { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTimeOffset? UpdatedDate { get; set; }

    public virtual ICollection<PaymentDetail> PaymentDetails { get; set; } = new List<PaymentDetail>();
}
