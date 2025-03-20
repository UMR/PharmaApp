using Pharmacy.Domain;

namespace Pharmacy.Application.Contracts.Persistence;

public interface ICustomerRepository
{
    Task<Customer> GetAsync(Guid customerId);
    Task<Customer> GetAsync(string mobile);
    ValueTask<bool> RegisterAsync(Customer customer);
}
