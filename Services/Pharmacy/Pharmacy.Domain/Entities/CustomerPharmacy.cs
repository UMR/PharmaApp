using System;
using System.Collections.Generic;

namespace Pharmacy.Domain;

public partial class CustomerPharmacy
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public Guid PharmacyId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Pharmacy Pharmacy { get; set; } = null!;
}
