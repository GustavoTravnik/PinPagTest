namespace BankServices.Dto
{
    public class ClientAccountBankMovementDto
    {
        public required BankMovementDto Movement { get; set; }
        public required ClientAccountMovimentDto Account { get; set; }
    }
}
