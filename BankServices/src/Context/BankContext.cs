using BankServices.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace BankServices.Context
{
    public class BankContext(DbContextOptions<BankContext> options) : DbContext(options)
    {
        public DbSet<ClientAccount> ClientAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientAccount>().ToTable("ClientAccount");
            modelBuilder.Entity<ClientAccount>().HasKey(x => x.Id);
            modelBuilder.Entity<ClientAccount>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<ClientAccount>().Property(x => x.Document).IsRequired();
            modelBuilder.Entity<ClientAccount>().Property(x => x.Amount).IsRequired();
        }
    }
}
