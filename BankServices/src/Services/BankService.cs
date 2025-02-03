using BankServices.Context;
using BankServices.Dto;
using BankServices.Entities;
using BankServices.Exceptions;
using BankServices.Utils;
using Microsoft.EntityFrameworkCore;
using static BankServices.Entities.BankTransaction;

namespace BankServices.Services
{
    public class BankService : IBankService
    {
        private readonly BankContext _context;

        public BankService(BankContext context)
        {
            _context = context;
        }

        public async Task CreateClientAccount(ClientAccountDto client)
        {
            if (client.Amount < 0)
            {
                throw new InvalidBankMovementException("The amount must be zero or greater.");
            }

            var validCpf = CpfValidator.GetValidCpf(client.Document) ?? throw new InvalidBankMovementException("The document(CPF) is invalid");
            var currentClientAccount = await _context.ClientAccounts.SingleOrDefaultAsync(c => c.Document == validCpf);
            if (currentClientAccount == null)
            {
                await _context.ClientAccounts.AddAsync(new ClientAccount { Name = client.Name, Document = validCpf, Amount = client.Amount });
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new DuplicatedClientAccountException("This client account alread exists.");
            }
        }

        public async Task<decimal> EditClientAccount(ClientAccountIdentifyerDto account, BankMovementDto movement)
        {
            if (movement.Amount <= 0)
            {
                throw new InvalidBankMovementException("The amount must be greater than zero.");
            }

            ClientAccountIdentifyerDto.ValidateParams(account);

            var currentClientAccount = await ValidadeAndGetClientAccountFromIdentifyerDto(account);

            if (movement.Type == TransactionTypes.In)
            {
                currentClientAccount.Amount += movement.Amount;
            }
            else if (movement.Type == TransactionTypes.Out)
            {
                var oldAmount = currentClientAccount.Amount;
                currentClientAccount.Amount -= movement.Amount;

                if (currentClientAccount.Amount < 0)
                {
                    throw new InvalidBankMovementException(
                        $"This account doesn't have balance for this Withdraw value.{Environment.NewLine}The current amount is: {oldAmount} and the requested amount is {movement.Amount}");
                }
            }
            else
            {
                throw new InvalidBankMovementException("Invalid movement type.");
            }

            await RegisterTransaction(currentClientAccount, movement);
            await _context.SaveChangesAsync();
            return currentClientAccount.Amount ?? 0;

        }

        public async Task<decimal> GetClientAmount(ClientAccountIdentifyerDto account)
        {
            ClientAccountIdentifyerDto.ValidateParams(account);
            var currentClientAccount = await ValidadeAndGetClientAccountFromIdentifyerDto(account);
            return currentClientAccount.Amount ?? 0;
        }

        public async Task<long> GetTotalOfMovementsByTimeFilter(DateTime startDate, DateTime endDate)
        {
            if (endDate.Hour == 0 && endDate.Minute == 0 && endDate.Second == 0)
            {
                endDate = endDate.AddDays(1).AddSeconds(-1);
            }

            return await _context.BankTransactions
                .Where(t => t.OccurredAt >= startDate && t.OccurredAt <= endDate)
                .CountAsync();
        }

        public async Task<PaginatedRespondeDto<ClientTransactionItem>> GetClientTransactionList(ClientAccountIdentifyerDto account, int pageNumber, int pageSize)
        {
            ClientAccountIdentifyerDto.ValidateParams(account);
            var currentClientAccount = await ValidadeAndGetClientAccountFromIdentifyerDto(account);
            var transactions = await _context.BankTransactions
                .Where(t => t.ClientAccountId == currentClientAccount.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ClientTransactionItem
                {
                    Amount = x.Amount,
                    OccurredAt = x.OccurredAt,
                    TransactionType = x.TransactionType
                }).ToListAsync();

            return new PaginatedRespondeDto<ClientTransactionItem>(transactions, transactions.Count, pageNumber, pageSize);
        }

        private async Task<ClientAccount> ValidadeAndGetClientAccountFromIdentifyerDto(ClientAccountIdentifyerDto account)
        {
            var parsedDocument = account?.Document;
            var accountId = account?.Id;
            return await _context.ClientAccounts.SingleOrDefaultAsync(c => c.Id == accountId || c.Document == CpfValidator.GetValidCpf(parsedDocument))
                        ?? throw new ClientAccountNotFoundException("Client account not found.");
        }

        private async Task RegisterTransaction(ClientAccount account, BankMovementDto movement)
        {
            await _context.BankTransactions.AddAsync(new BankTransaction
            {
                Amount = movement.Amount,
                ClientAccountId = account.Id,
                TransactionType = movement.Type,
            });
        }
    }
}
