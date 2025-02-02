using BankServices.Context;
using BankServices.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BankServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankOperation : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<BankOperation> _logger;
        private readonly BankContext _context;

        public BankOperation(ILogger<BankOperation> logger, BankContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost(Name = "New")]
        public StatusCodeResult New()
        {
            _context.ClientAccounts.Add(new ClientAccount { Name = "John Doe", Document = "12345678900", Amount = 1000 });
            _context.SaveChanges();
            return Ok();
        }
    }
}
