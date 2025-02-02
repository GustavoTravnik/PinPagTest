using BankServices.Dto;

namespace BankServices.Services
{
    public interface IBankServices
    {
        Task CreateClientAccount(ClientAccountDto client);
        Task<decimal> EditClientAccount(ClientAccountMovimentDto account, BankMovementDto movement);
    }
}
