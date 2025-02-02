namespace BankServices.Dto
{
    public class BankMovementDto
    {
        public enum MovementType
        {
            Deposit,
            Withdraw
        }

        public decimal Amount { get; set; }
        public required MovementType Type { get; set; }
    }
}
