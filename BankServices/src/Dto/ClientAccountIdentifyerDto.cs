using BankServices.Exceptions;

namespace BankServices.Dto
{
    public class ClientAccountIdentifyerDto
    {
        public string? Document { get; set; }
        public Guid? Id { get; set; }

        public static void ValidateParams(ClientAccountIdentifyerDto account)
        {
            if (account.Id != null && account.Document != null)
            {
                throw new InvalidBankMovementException("You must specify only one of account identifyer, Id or Document.");
            }
        }

        public static ClientAccountIdentifyerDto Parse(string identifyer)
        {
            if (Guid.TryParse(identifyer, out var newGuid))
            {
                return new ClientAccountIdentifyerDto { Id = newGuid };
            }
            else
            {
                return new ClientAccountIdentifyerDto { Document = identifyer };
            }
        }
    }
}
