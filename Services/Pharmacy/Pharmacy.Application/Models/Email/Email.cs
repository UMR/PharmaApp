namespace Pharmacy.Application.Models.Email
{
    public class Email
    {
        public string EmailSubject { get; set; } = string.Empty;
        public string EmailBody { get; set; } = string.Empty;
        public List<string> EmailAddress { get; set; } = [];
    }
}
