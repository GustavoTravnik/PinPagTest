using BankServices.Context;
using BankServices.Dto;
using BankServices.Exceptions;
using BankServices.Services;
using Microsoft.EntityFrameworkCore;
using static BankServices.Entities.BankTransaction;

namespace BankServicesTests
{
    //TODO : Implement the rest of unit tests... (A lot of possibilities)
    public class UnitTests
    {
        private readonly DbContextOptions<BankContext> _contextOptions;
        private readonly BankContext _context;
        private readonly BankService _service;

        public UnitTests()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            _contextOptions = GetInMemoryDatabaseOptions();
            _context = new BankContext(_contextOptions);
            _service = new BankService(_context);
        }

        [Fact]
        public async Task CreateClientAccount_HaveValidProperties_ExpectedToCreateUserAccount()
        {
            var person = new ClientAccountDto { Name = "Rafinha", Document = "914.847.850-45", Amount = 120 };
            await _service.CreateClientAccount(person);
            Assert.Single(_context.ClientAccounts.Where(x => x.Document == "91484785045"));
        }

        [Theory]
        [InlineData("123123123123123123123", 120)]
        [InlineData("11", -120)]
        public async Task CreateClientAccount_GiveInvalidPropertiesOrInsertSamePerson_ThrowsException(string document, decimal value)
        {
            var person = new ClientAccountDto { Name = "Rafinha", Document = document, Amount = value };

            await Assert.ThrowsAsync<InvalidBankMovementException>(() => _service.CreateClientAccount(person));

            Assert.Empty(_context.ClientAccounts);
        }

        [Fact]
        public async Task CreateClientAccount_InsertSamePerson_ThrowsException()
        {
            var person = new ClientAccountDto { Name = "Rafinha", Document = "335.625.880-03", Amount = 120 };
            await _service.CreateClientAccount(person);
            await Assert.ThrowsAsync<DuplicatedClientAccountException>(() => _service.CreateClientAccount(person));
        }

        [Theory]
        [InlineData(TransactionTypes.In, 120, 120)]
        [InlineData(TransactionTypes.Out, 120, 120)]
        public async Task EditClientAccount_HaveValidProperties_ExpectedToDoTheChangesOperations(TransactionTypes type, decimal amount, decimal initialAmount)
        {
            var CpfNumber = GenerateCPF();
            var person = new ClientAccountDto { Name = "Rafinha", Document = CpfNumber, Amount = initialAmount };
            await _service.CreateClientAccount(person);
            Assert.Single(_context.ClientAccounts.Where(x => x.Document == CpfNumber));
            await _service.EditClientAccount(new ClientAccountIdentifyerDto { Document = CpfNumber }, new BankMovementDto { Type = type, Amount = amount });
            Assert.Equal(_context.ClientAccounts
                .First(x => x.Document == CpfNumber).Amount, initialAmount + (type == TransactionTypes.Out ? -amount : amount));
        }

        [Fact]
        public async Task EditClientAccount_HaveValidProperties_ThrowsException()
        {
            var type = TransactionTypes.Out;
            var amount = 120;
            var initialAmount = 100;
            var CpfNumber = GenerateCPF();
            var person = new ClientAccountDto { Name = "Rafinha", Document = CpfNumber, Amount = initialAmount };
            await _service.CreateClientAccount(person);
            Assert.Single(_context.ClientAccounts.Where(x => x.Document == CpfNumber));

            await Assert.ThrowsAsync<InvalidBankMovementException>(() =>
                _service.EditClientAccount(new ClientAccountIdentifyerDto { Document = CpfNumber }, new BankMovementDto { Type = type, Amount = amount }));
        }


        private DbContextOptions<BankContext> GetInMemoryDatabaseOptions() =>
            new DbContextOptionsBuilder<BankContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

        private string GenerateCPF()
        {
            int sum = 0, remainder = 0;
            int[] multiplier1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplier2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

            Random random = new Random();
            string seed = random.Next(100000000, 999999999).ToString();

            for (int i = 0; i < 9; i++)
                sum += int.Parse(seed[i].ToString()) * multiplier1[i];

            remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;

            seed += remainder;
            sum = 0;

            for (int i = 0; i < 10; i++)
                sum += int.Parse(seed[i].ToString()) * multiplier2[i];

            remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;

            seed += remainder;
            return seed;
        }
    }
}