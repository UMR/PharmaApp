using AutoMapper;
using Org.BouncyCastle.Asn1.Ocsp;
using Pharmacy.Application.Common.Constants;
using Pharmacy.Application.Contracts.Infrastructure;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Exceptions;
using Pharmacy.Application.Features.Authentication.Validators;
using Pharmacy.Application.Features.Customer.Dtos;
using Pharmacy.Application.Features.Customer.Validators;
using Pharmacy.Application.Features.CustomerPharmacy.Services;
using Pharmacy.Application.Models;

namespace Pharmacy.Application.Features.Customer.Services;

public class CustomerService : ICustomerService
{
    #region Fields

    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerPharmacyService _customerPharmacyService;
    private readonly IIdentityService _identityService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    
    #endregion

    #region Ctor

    public CustomerService(ICustomerRepository customerRepository,
                            ICustomerPharmacyService customerPharmacyService,
                            IIdentityService identityService,
                            IServiceProvider serviceProvider,
                            IMapper mapper)
    {
        _customerRepository = customerRepository;
        _customerPharmacyService = customerPharmacyService;
        _identityService = identityService;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    public async Task<CustomerResDto> RegisterAsync(CustomerRegDto customerInfo)
    {
        var validator = new CustomerRegDtoValidator(_serviceProvider);
        var validationResult = await validator.ValidateAsync(customerInfo);

        if (validationResult.IsValid == false)
        {
            throw new ValidationRequestException(validationResult.Errors);
        }

        Domain.CustomerPharmacy? storedCustomerPharmacy = null;

        // consider the following scenerio
        // 1. Mobile number without "+" symbol at the begining
        // 2. User Update his/her info

        var storedCustomer = await _customerRepository.GetAsync(customerInfo.Mobile);

        if(storedCustomer == null)
        {
            storedCustomer = await CreateCustomerAsync(customerInfo);
        }

        if (storedCustomer != null)
        {
            storedCustomerPharmacy = await _customerPharmacyService.CreateAsync(storedCustomer.Id, customerInfo.PharmacyId);
        }

        if(storedCustomerPharmacy != null)
        {
            //var response = await _identityService.GetToken(customerInfo.Mobile, CustomerConstant.customerPassword);

            //if (response != null && !string.IsNullOrEmpty(response.AccessToken))
            //{
            //    return response;
            //}

            return _mapper.Map<CustomerResDto>(storedCustomer);
        }

        //return new TokenResponseDto();
        return null;
    }

    #endregion

    #region Private Method

    private async Task<Domain.Customer?> CreateCustomerAsync(CustomerRegDto customerInfo)
    {
        var customer = new Domain.Customer();

        customer.Id = Guid.NewGuid();
        customer.FirstName = customerInfo.FirstName.Trim();
        customer.LastName = customerInfo.LastName?.Trim();
        customer.Email = customerInfo.Email?.Trim();
        customer.Mobile = customerInfo.Mobile!.Trim();
        customer.Age = customerInfo.Age;
        customer.Weight = customerInfo.Weight;
        customer.CreatedBy = customer.Id;
        customer.CreatedDate = DateTime.UtcNow;

        bool response = await _customerRepository.RegisterAsync(customer);
        
        return response ? customer : null;
    }

    #endregion
}
