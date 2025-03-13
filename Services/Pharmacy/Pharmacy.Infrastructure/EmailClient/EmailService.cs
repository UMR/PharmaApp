using Microsoft.Extensions.Configuration;
using Pharmacy.Application.Contracts.Infrastructure;
using Pharmacy.Application.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Pharmacy.Infrastructure.EmailClient
{
    public class EmailService : IEmailService
    {
        #region Fields

        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public EmailService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        #endregion

        #region Methods

        public async Task SendEmailAsync(Email req)
        {
            using SmtpClient smtp = new SmtpClient();
            try
            {
                MimeMessage email = new MimeMessage();
                //{
                //    From = { MailboxAddress.Parse(_configuration.GetSection("EmailUsername").Value) }

                //};
                email.From.Add(new MailboxAddress("SCOHN", _configuration.GetSection("EmailUsername").Value));
                foreach (string emailAddress in req.EmailAddress)
                {
                    email.To.Add(MailboxAddress.Parse(emailAddress));
                }

                email.Subject = req.EmailSubject;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = req.EmailBody
                };
                smtp.Connect(_configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration.GetSection("EmailUsername").Value, _configuration.GetSection("EmailPassword").Value);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task SendErrorEmailAsync(Exception Message)
        {
            var email = new Email
            {
                EmailSubject = "This is an error Email from SCOHN!",
                EmailBody = "An Error Occured" +
                            "- Log" +
                            Message +
                            $"- Time: {DateTime.UtcNow}\n\n" +
                            $"- StackTrace: {Message.StackTrace}"
            };

            email.EmailAddress.AddRange(
            [
                ""

            ]);

            await SendEmailAsync(email);
        }

        #endregion
    }
}
