namespace BankServices.Exceptions
{
    [Serializable]
    internal class DuplicatedClientAccountException(string message) : BankApplicationException(message)
    {
    }
}
