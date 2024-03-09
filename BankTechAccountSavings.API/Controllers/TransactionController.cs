using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Application.Transactions.Interfaces;
using BankTechAccountSavings.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BankTechAccountSavings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(ITransactionService transactionService) : ControllerBase
    {
        public readonly ITransactionService _transactionService = transactionService;

        [HttpGet]
        public async Task<ActionResult<List<GetTransaction>>> GetTransactions()
        {
            try
            {
                List<GetTransaction> transactions = await _transactionService.GetTransactions();

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

        [HttpGet("paginated/transactions")]
        public async Task<ActionResult<Paginated<GetTransaction>>> GetPaginatedTransactions(int page, int pageSize)
        {
            try
            {
                Paginated<GetTransaction> paginatedResult = await _transactionService.GetPaginatedTransactionsAsync(page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound("No Transactions were found");
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("transfer")]
        public async Task<ActionResult<List<GetTransfer>>> GetAllTransfers()
        {
            try
            {
                List<GetTransfer>? transactions = await _transactionService.GetAllTransfers();

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

        [HttpGet("paginated/transfers")]
        public async Task<ActionResult<Paginated<GetTransfer>>> GetPaginatedTransfers(int page, int pageSize)
        {
            try
            {
                Paginated<GetTransfer> paginatedResult = await _transactionService.GetPaginatedTransfersAsync(page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound("No Transactions were found");
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{accountId:Guid}/paginated/transfers")]
        public async Task<ActionResult<Paginated<GetTransfer>>> GetPaginatedTransfersByAccount(Guid accountId, int page, int pageSize)
        {
            try
            {
                Paginated<GetTransfer> paginatedResult = await _transactionService.GetPaginatedTransfersByAccountAsync(accountId, page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound($"No Transfers were found for the account {accountId}");
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("transfer/{transactionId:guid}")]
        public async Task<ActionResult<GetTransfer>?> GetTransferbyId(Guid transactionId)
        {
            try
            {
                GetTransfer? transaction = await _transactionService.GetTransferbyId(transactionId);

                if (transaction == null)
                {
                    return NotFound("No transfers were found.");
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("transfer/account/{accountId:guid}")]
        public async Task<ActionResult<GetTransfer>?> GetAccountTransfers(Guid accountId)
        {
            try
            {
                GetTransfer? transaction = await _transactionService.GetTransferbyId(accountId);

                if (transaction == null)
                {
                    return NotFound("No transfers were found on the Account.");
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("deposit")]
        public async Task<ActionResult<List<GetDeposit>>> GetAllDeposits()
        {
            try
            {
                List<GetDeposit>? transactions = await _transactionService.GetAllDeposits();

                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound("No Deposits were found");
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("paginated/deposits")]
        public async Task<ActionResult<Paginated<GetDeposit>>> GetPaginatedDeposits(int page, int pageSize)
        {
            try
            {
                Paginated<GetDeposit> paginatedResult = await _transactionService.GetPaginatedDepositsAsync(page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound("No Transactions were found");
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{accountId:Guid}/paginated/deposits")]
        public async Task<ActionResult<Paginated<GetDeposit>>> GetPaginatedDepositsByAccount(Guid accountId, int page, int pageSize)
        {
            try
            {
                Paginated<GetDeposit> paginatedResult = await _transactionService.GetPaginatedDepositsByAccountAsync(accountId, page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound($"No Deposits were found for the account {accountId}");
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("deposit/{transactionId:guid}")]
        public async Task<ActionResult<GetDeposit>?> GetDepositbyId(Guid transactionId)
        {
            try
            {
                GetDeposit? transaction = await _transactionService.GetDepositbyId(transactionId);

                if (transaction == null)
                {
                    return NotFound("No deposit were found.");
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("deposit/account/{accountId:guid}")]
        public async Task<ActionResult<List<GetDeposit>>?> GetAccountDeposits(Guid accountId)
        {
            try
            {
                List<GetDeposit>? transaction = await _transactionService.GetAllDepositsByAccount(accountId);

                if (transaction == null)
                {
                    return NotFound("No deposits were found on the Account.");
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("withdraw")]
        public async Task<ActionResult<List<GetWithdraw>>> GetAllWitdraws()
        {
            try
            {
                List<GetWithdraw>? transactions = await _transactionService.GetAllWithdraws();

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

        [HttpGet("paginated/withdraws")]
        public async Task<ActionResult<Paginated<GetWithdraw>>> GetPaginatedWithdraws(int page, int pageSize)
        {
            try
            {
                Paginated<GetWithdraw> paginatedResult = await _transactionService.GetPaginatedWithdrawsAsync(page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound("No Transactions were found");
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{accountId:Guid}/paginated/withdraws")]
        public async Task<ActionResult<Paginated<GetWithdraw>>> GetPaginatedWithdrawsByAccount(Guid accountId, int page, int pageSize)
        {
            try
            {
                Paginated<GetWithdraw> paginatedResult = await _transactionService.GetPaginatedWithdrawsByAccountAsync(accountId, page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound($"No Deposits were found for the account {accountId}");
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("withdraw/{transactionId:guid}")]
        public async Task<ActionResult<GetWithdraw>?> GetWitdrawbyId(Guid transactionId)
        {
            try
            {
                GetWithdraw? transaction = await _transactionService.GetWithdrawbyId(transactionId);

                if (transaction == null)
                {
                    return NotFound("No transfers were found.");
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("withdraw/account/{accountId:guid}")]
        public async Task<ActionResult<GetWithdraw>?> GetAccountWithdraws(Guid accountId)
        {
            try
            {
                GetWithdraw? transaction = await _transactionService.GetWithdrawbyId(accountId);

                if (transaction == null)
                {
                    return NotFound("No withdraws were found on the Account.");
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
