using BankServices.Dto;

namespace BankServices.Services
{
    public interface IBankServices
    {
        Task CreateClientAccount(ClientAccountDto client);
        Task<decimal> EditClientAccount(ClientAccountIdentifyerDto account, BankMovementDto movement);
        Task<decimal> GetClientAmount(ClientAccountIdentifyerDto account);
        Task<PaginatedRespondeDto<ClientTransactionItem>> GetClientTransactionList(ClientAccountIdentifyerDto account, int pageNumber, int pageSize);
        Task<long> GetTotalOfMovementsByTimeFilter(DateTime startDate, DateTime endDate);
    }
}
