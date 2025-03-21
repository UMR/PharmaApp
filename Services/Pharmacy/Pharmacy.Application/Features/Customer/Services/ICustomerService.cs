﻿using Pharmacy.Application.Features.Customer.Dtos;
using Pharmacy.Application.Models;

namespace Pharmacy.Application.Features.Customer.Services;

public interface ICustomerService
{
    Task<CustomerResDto> RegisterAsync(CustomerRegDto customerInfo);
}
