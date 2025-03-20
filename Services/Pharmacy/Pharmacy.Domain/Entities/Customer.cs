using System;
using System.Collections.Generic;

namespace Pharmacy.Domain;

public partial class Customer
{
    public Guid Id { get; set; }

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

    public virtual ICollection<CustomerPharmacy> CustomerPharmacies { get; set; } = new List<CustomerPharmacy>();
}
