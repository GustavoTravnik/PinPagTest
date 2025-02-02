using BankServices.Context;
using BankServices.Dto;
using BankServices.Entities;
using BankServices.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BankServices.Services
{
    public class BankServices : IBankServices
    {
        private readonly BankContext _context;

        public BankServices(BankContext context)
        {
            _context = context;
        }

        public async Task CreateClientAccount(ClientAccountDto client)
        {
            var currentClientAccount = await _context.ClientAccounts.SingleOrDefaultAsync(c => c.Document == client.Document);
            if (currentClientAccount == null)
            {
                await _context.ClientAccounts.AddAsync(new ClientAccount { Name = client.Name, Document = client.Document, Amount = client.Amount });
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new DuplicatedClientAccountException("This client account alread exists.");
            }
        }

        public async Task<decimal> EditClientAccount(ClientAccountMovimentDto account, BankMovementDto movement)
        {
            if (movement.Amount <= 0)
            {
                throw new InvalidBankMovementException("The amount must be greater than zero.");
            }

            if (account.Id != null && account.Document != null)
            {
                throw new InvalidBankMovementException("You must specify only one of account identifyer, Id or Document.");
            }

            var currentClientAccount = await _context.ClientAccounts
                .SingleOrDefaultAsync(c => c.Id == account.Id || c.Document == account.Document);

            if (currentClientAccount != null)
            {
                if (movement.Type == BankMovementDto.MovementType.Deposit)
                {
                    currentClientAccount.Amount += movement.Amount;
                }
                else if (movement.Type == BankMovementDto.MovementType.Withdraw)
                {
                    var oldAmount = currentClientAccount.Amount;
                    currentClientAccount.Amount -= movement.Amount;

                    if (currentClientAccount.Amount < 0)
                    {
                        throw new InvalidBankMovementException(
                            $"This account doesn't have balance for this Withdraw value.{Environment.NewLine}The current amount is: {oldAmount} and the requested amount i {movement.Amount}");
                    }
                }
                else
                {
                    throw new InvalidBankMovementException("Invalid movement type.");
                }

                await _context.SaveChangesAsync();
                return currentClientAccount.Amount ?? 0;
            }
            else
            {
                throw new ClientAccountNotFoundException("Client account not found.");
            }
        }
    }
}
