namespace Pharmacy.Application.Constants
{
    public class RegexConstant
    {
        public const string SixDigitInteger = @"^\d{6}$";
        public const string Email = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        public static readonly string[] Mobile = [
                                    @"^(\+880|880)?1[3-9]\d{8}$",
                                @"^(\+91[ \-]?|91[ \-]?|0)?[6-9]\d{9}$",
                                @"^(\+1|1)?\d{10}$"
                                ];
        public const string Base64String = @"^[a-zA-Z0-9\+/]*={0,3}$";
    }
}
