﻿namespace Pharmacy.Application.Features.Customer.Dtos;

public class CustomerRegDto
{

    public Guid PharmacyId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string? Mobile { get; set; }

    public string? Email { get; set; }

    public short Age { get; set; }

    public float Weight { get; set; }
}
