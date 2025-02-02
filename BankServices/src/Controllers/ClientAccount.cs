using BankServices.Dto;
using BankServices.Exceptions;
using BankServices.Extentions;
using BankServices.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BankServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientAccount : Controller
    {
        private readonly IBankServices _bankServices;

        public ClientAccount(IBankServices bankServices)
        {
            _bankServices = bankServices;
        }

        [HttpPost("create")]
        [SwaggerOperation(Summary = "Creates a new client account")]
        public async Task<ActionResult> Create(ClientAccountDto client)
        {
            try
            {
                await _bankServices.CreateClientAccount(client);
                return Created();
            }
            catch (Exception ex)
            {
                if (ex.IsInternalExeption())
                {
                    return ValidationProblem(ex.Message);
                }

                return Problem(ex.Message);
            }
        }

        [HttpPut("edit")]
        [SwaggerOperation(Summary = "Make movement into an existing client account")]
        public async Task<ActionResult> Edit(ClientAccountBankMovementDto operation)
        {
            try
            {
                var newAccountAmount = await _bankServices.EditClientAccount(operation.Account, operation.Movement);
                return Ok($"Operation is done, and the new account amount is {newAccountAmount}");
            }
            catch (Exception ex)
            {
                if (ex.IsInternalExeption())
                {
                    return ValidationProblem(ex.Message);
                }

                return Problem(ex.Message);
            }
        }
    }
}
