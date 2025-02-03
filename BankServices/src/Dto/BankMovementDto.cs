using static BankServices.Entities.BankTransaction;

namespace BankServices.Dto
{
    public class BankMovementDto
    {
        public decimal Amount { get; set; }
        public required TransactionTypes Type { get; set; }
    }
}
