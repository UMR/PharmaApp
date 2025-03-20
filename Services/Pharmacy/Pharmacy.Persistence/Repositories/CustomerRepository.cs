using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Domain;

namespace Pharmacy.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    #region Fields

    private readonly PharmaAppDbContext _context;

    #endregion

    #region Ctor

    public CustomerRepository(PharmaAppDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async Task<Customer> GetAsync(Guid customerId)
    {
        var result = await _context.Customers.AsNoTracking().Where(c => c.Id == customerId).FirstOrDefaultAsync();

        return result;
    }

    public async Task<Customer> GetAsync(string mobile)
    {
        var result = await _context.Customers.AsNoTracking().Where(c => c.Mobile == mobile).FirstOrDefaultAsync();

        return result;
    }

    public async ValueTask<bool> RegisterAsync(Customer customer)
    { 
        await _context.Customers.AddAsync(customer);
        var result = await _context.SaveChangesAsync();

        return (result > 0) ? true : false;              
    }

    #endregion
}
