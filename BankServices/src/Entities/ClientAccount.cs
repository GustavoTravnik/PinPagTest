using Microsoft.EntityFrameworkCore;

namespace BankServices.Entities
{
    public class ClientAccount
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Document { get; set; }
        public decimal? Amount { get; set; }

        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientAccount>().ToTable("ClientAccount");
            modelBuilder.Entity<ClientAccount>().HasKey(x => x.Id);
            modelBuilder.Entity<ClientAccount>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<ClientAccount>().Property(x => x.Document).IsRequired();
            modelBuilder.Entity<ClientAccount>().Property(x => x.Amount).IsRequired();
        }
    }
}