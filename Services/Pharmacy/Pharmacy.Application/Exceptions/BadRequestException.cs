namespace Pharmacy.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public object Error { get; }

        public BadRequestException(object error)
            : base("A bad request error occurred.")
        {
            Error = error;

        }

        public BadRequestException(string name, object key) : base($"{name} ({key}) was not found")
        {}
    }
}
