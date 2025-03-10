using System;
using System.Collections.Generic;

namespace Pharmacy.Domain;

public partial class UserLogin
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime LogInTime { get; set; }

    public DateTime? LogOutTime { get; set; }

    public virtual User User { get; set; } = null!;
}
