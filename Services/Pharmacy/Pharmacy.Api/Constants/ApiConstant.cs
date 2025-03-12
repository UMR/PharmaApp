namespace Pharmacy.Api.Constants
{
    public static class ApiConstant
    {
        public const string CorsPolicy = "CorsPolicy";
    }

    public class IdentityServerClient
    {
        public List<string> AllowedCorsOrigins { get; set; }
    }
}
