using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pharmacy.Application.Common.Enums;
using Pharmacy.Application.Contracts.Infrastructure;
using Pharmacy.Application.Contracts.Persistence;
using Pharmacy.Application.Exceptions;
using Pharmacy.Application.Models;
using Pharmacy.Application.Models.Email;
using Pharmacy.Application.Models.SMS;
using Pharmacy.Application.Wrapper;
using Pharmacy.Domain;

namespace Pharmacy.Infrastructure.Otp
{
    public class OtpService : IOtpService
    {
        #region Fields

        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly Random _random;
        private readonly IEmailService _emailService;
        private readonly ISMSService _smsService;
        private readonly IUserRepository _userRepository;
        private ILogger<OtpService> _logger;

        #endregion

        #region Ctro

        public OtpService(IMemoryCache cache,
                        IConfiguration configuration,
                        IEmailService emailService,
                        ISMSService smsService,
                        IUserRepository userRepository,
                        ILogger<OtpService> logger)
        {
            _cache = cache;
            _random = new Random();
            _configuration = configuration;
            _emailService = emailService;
            _smsService = smsService;
            _userRepository = userRepository;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        public async Task<BaseQueryResponse<OtpResponse>> GenerateOtp(string loginId, OtpType? type)
        {
            if (type == null || type == OtpType.Login)
            {
                return await GenerateOtpForLogin(loginId);
            }
            else if (type == OtpType.Forgot)
            {
                return await GenerateOtpForForgot(loginId);
            }

            return new BaseQueryResponse<OtpResponse>(new OtpResponse { Otp = null, ExpireTimeInSecond = 0 }, false);
        }

        public BaseQueryResponse<string> VerifyOtp(string loginId, string otp, OtpType? type)
        {
            if (type == null || type == OtpType.Login)
            {
                if (_cache.TryGetValue(loginId, out string cachedOtp))
                {
                    if (cachedOtp == otp)
                    {
                        _cache.Remove(loginId);
                        return new BaseQueryResponse<String>("OTP is valid", true);
                    }
                }
            }
            else if (type == OtpType.Forgot)
            {
                if (_cache.TryGetValue($"{loginId}{otp}", out string cachedOtp))
                {
                    if (cachedOtp == otp)
                    {
                        return new BaseQueryResponse<String>("OTP is valid", true);
                    }
                }
            }

            return new BaseQueryResponse<String>("OTP is invalid");
        }

        #endregion

        #region Private Methods

        private async Task<BaseQueryResponse<OtpResponse>> GenerateOtpForLogin(string loginId)
        {
            int expireTimeInSecond = Convert.ToInt32(_configuration["OtpTimeInSecond"]);
            string otp = _random.Next(100000, 999999).ToString();

            try
            {
                if (loginId.Contains("@"))
                {
                    string emailBody = $@"
                <html>
                <body>
                    <div style='display: flex; align-items: center; background-color: #f5f5f5; padding: 20px; border-radius: 10px;'>
                        <h1 style='margin: 0; font-size: 24px; color: #333;'>SCOHN <sup style='font-size: 8px;'>TM</h6></sup></h1>
                    </div>
                    <p>Your one-time password (OTP) is <strong><u style='color:red;'>{otp}</u></strong>.</p>
                    <p>This OTP is valid for {expireTimeInSecond / 60} minutes. Please do not share this OTP with anyone.</p>
                </body>
                </html>";

                    //await _emailService.SendEmailAsync(new Email
                    //{
                    //    EmailAddress = [loginId],
                    //    EmailSubject = "Verify Your SCOHN Account with this OTP",
                    //    EmailBody = emailBody
                    //});
                }

                else
                {
                    var mobileNumber = loginId;
                    if (loginId.Length == 10)
                    {
                        mobileNumber = "91" + mobileNumber;
                    }
                    //await _smsService.SendSMSAsync(new SMS
                    //{
                    //    SMSAddress = [mobileNumber],
                    //    SMSSubject = "OTP for verification",
                    //    SMSBody = $"{otp}"
                    //});
                }

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireTimeInSecond)
                };

                _cache.Set(loginId, otp, cacheOptions);
            }
            catch (Exception ex)
            {
                await _emailService.SendErrorEmailAsync(ex);
                _logger.LogError(ex, "Error while sending email");
            }

            var otpResponse = new OtpResponse { Otp = otp, ExpireTimeInSecond = expireTimeInSecond };

            return new BaseQueryResponse<OtpResponse>(otpResponse, true);
        }

        private async Task<BaseQueryResponse<OtpResponse>> GenerateOtpForForgot(string loginId)
        {
            var user = await _userRepository.GetAsync(loginId);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), loginId.ToString());
            }

            int expireTimeInSecond = Convert.ToInt32(_configuration["ForgotOtpTimeInSecond"]);
            string otp = _random.Next(100000, 999999).ToString();

            try
            {
                if (loginId.Contains("@"))
                {
                    //await _emailService.SendEmailAsync(new Email
                    //{
                    //    EmailAddress = [loginId],
                    //    EmailSubject = "OTP for verification",
                    //    EmailBody = $"Your OTP is {otp}. It will expire in {expireTimeInSecond}"
                    //});
                }
                else
                {
                    //await _smsService.SendSMSAsync(new SMS
                    //{
                    //    SMSAddress = [loginId],
                    //    SMSSubject = "OTP for verification",
                    //    SMSBody = $"{otp}"
                    //});
                }

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireTimeInSecond)
                };

                _cache.Set($"{loginId}{otp}", otp, cacheOptions);
            }
            catch (Exception ex)
            {
                await _emailService.SendErrorEmailAsync(ex);
                _logger.LogError(ex, "Error while sending email");
            }

            var otpResponse = new OtpResponse { Otp = otp, ExpireTimeInSecond = expireTimeInSecond };

            return new BaseQueryResponse<OtpResponse>(otpResponse, true);
        }

        #endregion
    }
}
