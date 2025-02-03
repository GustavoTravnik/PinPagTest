using BankServices.Dto;
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
        private readonly IBankService _bankServices;

        public ClientAccount(IBankService bankServices)
        {
            _bankServices = bankServices;
        }

        [HttpGet("getAmount/{identifyer}")]
        [SwaggerOperation(Summary = "Get the current amount of a client account")]
        public async Task<ActionResult> GetAmount(string identifyer)
        {
            try
            {
                return Ok(await _bankServices.GetClientAmount(ClientAccountIdentifyerDto.Parse(identifyer)));
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

        [HttpGet("getTransactionList/{identifyer}")]
        [SwaggerOperation(Summary = "Get a list of back client transactions")]
        public async Task<ActionResult> GetAmount(string identifyer, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(await _bankServices.GetClientTransactionList(ClientAccountIdentifyerDto.Parse(identifyer), pageNumber, pageSize));
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

        //[HttpGet("{startDate}/{endDate}")]
        [HttpGet("getTotalOperations")]
        [SwaggerOperation(Summary = "Get the amount of movements with datetime filter")]
        public async Task<ActionResult> GetTotalOperations(
            [SwaggerParameter("Start date parameter as ISO 8601 format")][FromQuery] DateTime startDate,
            [SwaggerParameter("end date parameter as ISO 8601 format")][FromQuery] DateTime endDate)
        {
            try
            {
                return Ok(await _bankServices.GetTotalOfMovementsByTimeFilter(startDate, endDate));
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
