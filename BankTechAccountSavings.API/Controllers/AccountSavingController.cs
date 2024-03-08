using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BankTechAccountSavings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountSavingController(IAccountSavingService accountSavingService) : ControllerBase
    {
        private readonly IAccountSavingService _accountService = accountSavingService;

        [HttpGet()]
        public async Task<ActionResult<List<GetAccountSaving>>> GetAccounts()
        {
            try
            {
                List<GetAccountSaving> accountSavings = await _accountService.GetAllAccountsAsync();

                if (accountSavings == null || accountSavings.Count == 0)
                {
                    return NotFound("No Accounts were found");
                }

                return Ok(accountSavings);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("{accountId:Guid}")]
        public async Task<ActionResult<GetAccountSaving>> GetAccountById(Guid accountId)
        {

            try
            {
                GetAccountSaving? account = await _accountService.GetAccountSavingByIdAsync(accountId);

                if (account == null)
                {
                    return NotFound("No Account found.");
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("account-number/{accountNumber:long}")]
        public async Task<ActionResult<GetAccountSaving>> GetAccountByAccountNumber(long accountNumber)
        {
            try
            {
                GetAccountSaving? account = await _accountService.GetAccountSavingByAccountNumberAsync(accountNumber);

                if (account == null)
                {
                    return NotFound("No Account found.");
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpGet("{accountId:Guid}/transactions")]
        public async Task<ActionResult<List<GetTransaction>>> GetTransactionHistory(Guid accountId)
        {
            try
            {
                List<GetTransaction>? transactions = await _accountService.GetTransactionsHistory(accountId);

                if (transactions == null || transactions.Count == 0)
                {
                    return NotFound("No Transactions found.");
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("{accountId:Guid}/deposit")]
        public async Task<ActionResult<GetTransaction>?> AddDeposit(int amount, Guid accountId, string description, TransactionType transactionType)
        {
            try
            {
                GetTransaction? deposit = await _accountService.AddDepositAsync(amount, accountId, description, transactionType);

                if (deposit == null)
                {
                    return NotFound("Deposit failed.");
                }

                return Ok(deposit);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("{accountId:Guid}/withdraw")]
        public async Task<ActionResult<GetTransaction>?> AddWithDraw(int amount, Guid accountId)
        {
            try
            {
                GetTransaction? deposit = await _accountService.WithDrawAsync(amount, accountId);

                if (deposit == null)
                {
                    return NotFound("WithDraw failed.");
                }

                return Ok(deposit);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpPost("{fromAccountId:Guid}/transfer/{toAccountId:Guid}")]
        public async Task<ActionResult<GetTransaction>?> TransferFunds(Guid fromAccountId, Guid toAccountId, int transferAmount, TransactionType transactionType)
        {
            try
            {
                GetTransaction? transaction = await _accountService.TransferFunds(fromAccountId, toAccountId, transferAmount, transactionType);

                if (transaction == null)
                {
                    return NotFound("Transaction failed.");
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        public async Task<ActionResult<CreatedAccountSavingResponse>?> CreateAccount(CreateAccountSaving createAccount)
        {
            try
            {
                CreatedAccountSavingResponse? account = await _accountService.CreateAccountSavingAsync(createAccount);

                if (account == null)
                {
                    return NotFound("Failed to create the account");
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut("{accountId:Guid}")]
        public async Task<ActionResult<UpdatedAccountSavingResponse>?> UpdateAccount(Guid accountId, UpdateAccountSaving updateAccountSaving)
        {
            try
            {
                UpdatedAccountSavingResponse? account = await _accountService.UpdateAccountSavingAsync(accountId, updateAccountSaving);

                if (account == null)
                {
                    return NotFound("Failed to update the account");
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPatch("{accountId:Guid}/close")]
        public async Task<ActionResult<DeletedAccountSavingResponse>?> CloseAccount(Guid accountId)
        {
            try
            {
                DeletedAccountSavingResponse? account = await _accountService.CloseAccountSavingAsync(accountId);

                if (account == null)
                {
                    return NotFound("Failed to closed the account");
                }

                return Ok("Account is closed");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete("{accountId:Guid}")]
        public async Task<ActionResult<DeletedAccountSavingResponse>?> DeleteAccount(Guid accountId)
        {
            try
            {
                DeletedAccountSavingResponse? account = await _accountService.DeleteAccountSavingAsync(accountId);

                if (account == null)
                {
                    return NotFound("Failed to delete the account");
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

    }
}
