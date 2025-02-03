namespace BankServices.Exceptions
{
    [Serializable]
    public class InvalidBankMovementException(string? message) : BankApplicationException(message)
    {
    }
}