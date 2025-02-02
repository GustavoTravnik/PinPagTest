namespace BankServices.Exceptions
{
    public class BankApplicationException(string message) : Exception(message)
    {
        public bool IsBusinessException { get; set; } = true;
    }
}
