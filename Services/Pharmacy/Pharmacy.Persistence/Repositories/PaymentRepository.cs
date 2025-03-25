using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Domain;

namespace Pharmacy.Persistence.Repositories;

public class PaymentRepository : IPaymentRepository
{
    #region Fields

    private readonly PharmaAppDbContext _context;

    #endregion

    #region Ctor

    public PaymentRepository(PharmaAppDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    public async ValueTask<bool> CreatePaymentAsync(PaymentDetail paymentDetail)
    {
        await _context.PaymentDetails.AddAsync(paymentDetail);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    #endregion
}
