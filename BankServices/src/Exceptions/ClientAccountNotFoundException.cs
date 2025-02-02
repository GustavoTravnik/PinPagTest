namespace BankServices.Exceptions
{
    [Serializable]
    internal class ClientAccountNotFoundException(string? message) : BankApplicationException(message)
    {
    }
}