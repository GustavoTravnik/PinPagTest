namespace BankServices.Exceptions
{
    [Serializable]
    public class DuplicatedClientAccountException(string message) : BankApplicationException(message)
    {
    }
}
