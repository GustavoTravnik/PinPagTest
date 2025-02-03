using Microsoft.EntityFrameworkCore;
using System.Runtime.Serialization;

namespace BankServices.Entities
{
    public class BankTransaction
    {
        public enum TransactionTypes { [EnumMember(Value = "Deposit")] In, [EnumMember(Value = "Withdraw")] Out }

        public Guid Id { get; set; }
        public Guid ClientAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset? OccurredAt { get; set; }
        public TransactionTypes TransactionType { get; set; }

        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankTransaction>().ToTable("BankTransactions");
            modelBuilder.Entity<BankTransaction>().HasKey(x => x.Id);
            modelBuilder.Entity<BankTransaction>().Property(x => x.ClientAccountId).IsRequired();
            modelBuilder.Entity<BankTransaction>().Property(x => x.Amount).IsRequired();
            modelBuilder.Entity<BankTransaction>().Property(x => x.OccurredAt).HasDefaultValueSql("NOW()");
            modelBuilder.Entity<BankTransaction>().Property(x => x.TransactionType);
        }
    }
}
