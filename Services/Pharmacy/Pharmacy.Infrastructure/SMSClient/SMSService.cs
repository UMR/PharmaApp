namespace Pharmacy.Infrastructure.SMSClient;

public class SMSService : ISMSService
{
    #region Fields

    private readonly IConfiguration _configuration;

    #endregion

    #region Ctro

    public SMSService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region Methods

    public Task SendErrorSMSAsync(Exception message)
    {
        throw new NotImplementedException();
    }

    public async Task<string> SendSMSAsync(SMS sms)
    {
        try
        {
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues(_configuration.GetSection("SMSAUTHURI").Value, new NameValueCollection()
                   {
                        { "apikey" , _configuration.GetSection("SMSAPIKey").Value },
                        { "numbers" , sms.SMSAddress[0].ToString() },
                        { "message" , sms.SMSBody + " OTP for Login Transaction on Universal Medical Record is valid for 2 minutes. For security reasons, please do not share the OTP with anyone." },
                        { "sender" , "UMRPL" }
                   }
                );
                string result = System.Text.Encoding.UTF8.GetString(response);
                return result;
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    #endregion
}
