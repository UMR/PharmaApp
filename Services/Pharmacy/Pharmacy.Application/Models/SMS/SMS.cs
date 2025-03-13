namespace Pharmacy.Application.Models.SMS
{
    public class SMS
    {
        public string SMSSubject { get; set; } = string.Empty;
        public string SMSBody { get; set; } = string.Empty;
        public List<string> SMSAddress { get; set; } = [];
    }
}
