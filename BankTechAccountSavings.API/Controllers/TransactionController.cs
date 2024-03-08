using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Application.Transactions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankTechAccountSavings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(ITransactionService  transactionService) : ControllerBase
    {
        public readonly ITransactionService _transactionService = transactionService;

        [HttpGet]
        public async Task<ActionResult<List<GetTransaction>>> GetTransactions()
        {
            try
            {
                List<GetTransaction> transactions = await _transactionService.GetAllAsync();

                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound("No Transactions were found");
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("{transactionId:guid}")]
        public async Task<ActionResult<GetTransaction>?> GetTransactionById(Guid transactionId)
        {
            try
            {
                GetTransaction? transaction = await _transactionService.GetbyIdAsync(transactionId);

                if (transaction == null)
                {
                    return NotFound("No transactions found.");
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

    }
}
