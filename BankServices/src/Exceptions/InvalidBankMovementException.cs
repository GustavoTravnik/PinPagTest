namespace BankServices.Exceptions
{
    [Serializable]
    internal class InvalidBankMovementException(string? message) : BankApplicationException(message)
    {
    }
}