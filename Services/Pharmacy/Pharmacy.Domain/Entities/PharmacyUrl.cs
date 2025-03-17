using System;
using System.Collections.Generic;

namespace Pharmacy.Domain;

public partial class PharmacyUrl
{
    public long Id { get; set; }

    public string Url { get; set; } = null!;

    public Guid PharmacyId { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Pharmacy Pharmacy { get; set; } = null!;
}
