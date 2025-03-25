using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;

namespace Pharmacy.Infrastructure.Payment.Razorpay;
public class RazorpayGatewayService : IRazorpayGatewayService
{
    #region Fields

    private readonly IConfiguration _configuration;
    private readonly string _key;
    private readonly string _secret;
    private RazorpayClient _client;

    #endregion

    #region Ctor

    public RazorpayGatewayService(IConfiguration configuration)
    {
        _configuration = configuration;
        _key = _configuration.GetSection("RazorPay").GetValue<string>("keyId");
        _secret = _configuration.GetSection("RazorPay").GetValue<string>("keySecret");
        _client = new RazorpayClient(_key, _secret);
    }

    #endregion

    #region Methods

    public string CreateOrder(decimal amount, string currency)
    {
        Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", amount * 100 },
            { "receipt", new Random().Next(10000000, 100000000).ToString() },
            { "currency", currency },
            { "payment_capture", "0" }, // 1 - automatic, 0 - manual
        };

        try
        {
            var order = _client.Order.Create(options);
            var orderId = order["id"].ToString();

            return orderId;
        }
        catch
        {
            return null;
            throw;
        }
    }

    public object GetKey()
    {
        throw new NotImplementedException();
    }

    public object GetPaymentDetails(string paymentId)
    {
        throw new NotImplementedException();
    }

    public bool VerifyPayment(string orderId, string paymentId, string signature)
    {
        throw new NotImplementedException();
    }

    #endregion
}
