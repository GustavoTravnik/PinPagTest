namespace BankServices.Exceptions
{
    [Serializable]
    public class ClientAccountNotFoundException(string? message) : BankApplicationException(message)
    {
    }
}