namespace BankServices.Dto
{
    public class ClientAccountDto
    {
        public required string Name { get; set; }
        public required string Document { get; set; }
        public decimal Amount { get; set; }
    }
}
