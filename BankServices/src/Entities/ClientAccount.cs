namespace BankServices.Entities
{
    public class ClientAccount
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Document { get; set; }
        public decimal? Amount { get; set; }

    }
}