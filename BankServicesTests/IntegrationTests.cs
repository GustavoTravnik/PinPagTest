using BankServices.Context;
using BankServices.Controllers;
using BankServices.Dto;
using BankServices.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace BankServicesTests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<ClientAccount>>
    {
        private readonly HttpClient _client;
        private readonly DbContextOptions<BankContext> _contextOptions;
        private readonly BankContext _context;
        private readonly BankService _service;

        public IntegrationTests(WebApplicationFactory<ClientAccount> factory)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            _client = factory.CreateClient();
            _contextOptions = GetInMemoryDatabaseOptions();
            _context = new BankContext(_contextOptions);
            _service = new BankService(_context);
        }

        [Fact]
        public async Task EnsureSuccessOf_ClientAccount_getAmount()
        {
            var person = new ClientAccountDto { Name = "Rafinha", Document = "857.521.930-87", Amount = 100 };

            await _service.CreateClientAccount(person);

            var response = await _client.GetAsync("/api/ClientAccount/getAmount/857.521.930-87");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("100", content);
        }

        [Fact]
        public async Task EnsureFailuresOf_ClientAccount_getAmount()
        {
            var person = new ClientAccountDto { Name = "Rafinha", Document = "964.915.930-45", Amount = 100 };

            await _service.CreateClientAccount(person);

            var response = await _client.GetAsync("/api/ClientAccount/getAmount/964.915.930-42");

            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task EnsureSuccessOf_ClientAccount_getTransactionList()
        {
            var person = new ClientAccountDto { Name = "Rafinha", Document = "127.330.270-26", Amount = 100 };

            await _service.CreateClientAccount(person);

            var response = await _client.GetAsync("/api/ClientAccount/getTransactionList/127.330.270-26?pageNumber=1&pageSize=2");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task EnsureFailuresOf_ClientAccount_getTransactionList()
        {
            var response = await _client.GetAsync("/api/ClientAccount/getTransactionList/5?pageNumber=1&pageSize=2");

            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task EnsureSuccessOf_ClientAccount_getTotalOperations()
        {
            var response = await _client.GetAsync("/api/ClientAccount/getTotalOperations?startDate=2025-03-02&endDate=2025-03-05");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task EnsureFailuresOf_ClientAccount_getTotalOperations()
        {
            var response = await _client.GetAsync("/api/ClientAccount/getTotalOperations?startDate=232222qqq&endDate=2025-03-05");

            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task EnsureSuccessOf_ClientAccount_create()
        {
            var person = new ClientAccountDto { Name = "Rafinha", Document = "322.724.340-70", Amount = 100 };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/ClientAccount/create", jsonContent);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task EnsureFailuresOf_ClientAccount_create()
        {
            var person = new ClientAccountDto { Name = "Rafinha", Document = "922.050.240-27", Amount = 100 };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/ClientAccount/create", jsonContent);

            Assert.True(response.IsSuccessStatusCode);

            response = await _client.PostAsync("/api/ClientAccount/create", jsonContent);

            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task EnsureSuccessOf_ClientAccount_edit()
        {
            var person = new ClientAccountDto { Name = "Rafinha", Document = "302.358.470-20", Amount = 100 };

            await _service.CreateClientAccount(person);

            var operation = new ClientAccountBankMovementDto
            {
                Account = new ClientAccountIdentifyerDto { Document = "302.358.470-20" },
                Movement = new BankMovementDto { Amount = 10, Type = BankServices.Entities.BankTransaction.TransactionTypes.In }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(operation), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("/api/ClientAccount/edit", jsonContent);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task EnsureFailuresOf_ClientAccount_edit()
        {
            var person = new ClientAccountDto { Name = "Rafinha", Document = "768.103.170-74", Amount = 0 };

            await _service.CreateClientAccount(person);

            var operation = new ClientAccountBankMovementDto
            {
                Account = new ClientAccountIdentifyerDto { Document = "768.103.170-74" },
                Movement = new BankMovementDto { Amount = 10, Type = BankServices.Entities.BankTransaction.TransactionTypes.Out }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(operation), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("/api/ClientAccount/edit", jsonContent);

            Assert.False(response.IsSuccessStatusCode);
        }

        private DbContextOptions<BankContext> GetInMemoryDatabaseOptions() =>
            new DbContextOptionsBuilder<BankContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
    }
}
