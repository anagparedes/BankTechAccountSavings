using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Application.Transactions.Validators;
using BankTechAccountSavings.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Identity.Client;
using BankTechAccountSavings.Application.AccountSavings.Services;
using BankTechAccountSavings.Domain.Entities;

namespace BankTechAccountSavings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountSavingController(IAccountSavingService accountSavingService, IValidator<CreateAccountSaving> createAccountSavingValidator, IValidator<UpdateAccountSaving> updateAccountSavingValidator, IValidator<CreateDeposit> createDepositValidator, IValidator<CreateWithdraw> createWithdrawValidator, IValidator<CreateTransfer> createTransferValidator) : ControllerBase
    {
        private readonly IAccountSavingService _accountService = accountSavingService;
        private readonly IValidator<CreateAccountSaving> _createAccountSavingValidator = createAccountSavingValidator;
        private readonly IValidator<UpdateAccountSaving> _updateAccountSavingValidator = updateAccountSavingValidator;
        private readonly IValidator<CreateDeposit> _createDeposit = createDepositValidator;
        private readonly IValidator<CreateWithdraw> _createWithdraw = createWithdrawValidator;
        private readonly IValidator<CreateTransfer> _createTransfer = createTransferValidator;

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

        [HttpGet("paginated")]
        public async Task<ActionResult<Paginated<GetAccountSaving>>> GetPaginatedAccounts(int page, int pageSize)
        {
            try
            {
                Paginated<GetAccountSaving> paginatedResult = await _accountService.GetPaginatedAccountsAsync(page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound("No Accounts were found");
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{accountId:Guid}/paginated/transactions")]
        public async Task<ActionResult<Paginated<GetTransaction>>> GetPaginatedTransactions(Guid accountId, int page, int pageSize)
        {
            try
            {
                Paginated<GetTransaction> paginatedResult = await _accountService.GetPaginatedTransactionsByAccountAsync(accountId, page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound($"No Transactions were found for the account {accountId}");
                }

                return Ok(paginatedResult);
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
        public async Task<ActionResult<GetDeposit>?> AddDeposit(int amount, Guid accountId, string description)
        {
            var createDeposit = new CreateDeposit
            {
                Amount = amount,
                DestinationProductId = accountId,
                Description = description
            };
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createDeposit, _createDeposit);
                if (validationResult != null)
                {
                    return validationResult;
                }
                GetDeposit? deposit = await _accountService.AddDepositAsync(amount, accountId, description);

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
        public async Task<ActionResult<GetWithdraw>?> AddWithDraw(int amount, Guid accountId)
        {
            var createWithdraw = new CreateWithdraw
            {
                SourceProductId = accountId,
                Amount = amount,
            };
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createWithdraw, _createWithdraw);
                if (validationResult != null)
                {
                    return validationResult;
                }
                GetWithdraw? withdraw = await _accountService.WithDrawAsync(amount, accountId);

                if (withdraw == null)
                {
                    return NotFound("WithDraw failed.");
                }

                return Ok(withdraw);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPost("{fromAccountId:Guid}/transfer/{toAccountId:Guid}")]
        public async Task<ActionResult<GetTransfer>?> TransferFunds(Guid fromAccountId, Guid toAccountId, string description, int transferAmount, TransferType transactionType)
        {
            var createTransfer = new CreateTransfer
            {
                SourceProductId = fromAccountId,
                DestinationProductId = toAccountId,
                Description = description,
                Amount = transferAmount,
                TransactionType = transactionType

            };
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createTransfer, _createTransfer);
                if (validationResult != null)
                {
                    return validationResult;
                }
                GetTransfer? transaction = await _accountService.TransferFunds(fromAccountId, toAccountId, description, transferAmount, transactionType);

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
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createAccount, _createAccountSavingValidator);
                if (validationResult != null)
                {
                    return validationResult;
                }

                CreatedAccountSavingResponse? account = await _accountService.CreateAccountSavingAsync(createAccount);

                if (account == null)
                {
                    return NotFound("Failed to create the account.");
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
                ActionResult? validationResult = await ValidateAndReturnResultAsync(updateAccountSaving, _updateAccountSavingValidator);
                if (validationResult != null)
                {
                    return validationResult;
                }
                UpdatedAccountSavingResponse? account = await _accountService.UpdateAccountSavingAsync(accountId, updateAccountSaving);

                if (account == null)
                {
                    return NotFound("Failed to update the account.");
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
                    return NotFound("Failed to closed the account.");
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
                    return NotFound("Failed to delete the account.");
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        private static async Task<ActionResult?> ValidateAndReturnResultAsync<T>(T model, IValidator<T> validator)
        {
            var validationResult = await validator.ValidateAsync(model);

            if (validationResult.IsValid)
            {
                return null;
            }

            var firstErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage;
            if (!string.IsNullOrEmpty(firstErrorMessage))
            {
                var formattedResult = $"\"errorMessage\": \"{firstErrorMessage}\"";
                return new BadRequestObjectResult(formattedResult);
            }
            return new BadRequestObjectResult(firstErrorMessage);
        }
    }
}
