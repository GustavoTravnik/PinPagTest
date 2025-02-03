using static BankServices.Entities.BankTransaction;

namespace BankServices.Dto
{
    public class ClientTransactionItem
    {
        public decimal Amount { get; set; }
        public DateTimeOffset? OccurredAt { get; set; }
        public TransactionTypes TransactionType { get; set; }
    }
}
