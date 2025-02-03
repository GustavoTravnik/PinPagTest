using BankServices.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankServices.Context
{
    public class BankContext(DbContextOptions<BankContext> options) : DbContext(options)
    {
        public DbSet<ClientAccount> ClientAccounts { get; set; }
        public DbSet<BankTransaction> BankTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<BankTransaction.TransactionTypes>("transactiontypes");

            ClientAccount.Configure(modelBuilder);
            BankTransaction.Configure(modelBuilder);
        }
    }
}
