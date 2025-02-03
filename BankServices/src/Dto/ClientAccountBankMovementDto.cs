namespace BankServices.Dto
{
    public class ClientAccountBankMovementDto
    {
        public required BankMovementDto Movement { get; set; }
        public required ClientAccountIdentifyerDto Account { get; set; }
    }
}
