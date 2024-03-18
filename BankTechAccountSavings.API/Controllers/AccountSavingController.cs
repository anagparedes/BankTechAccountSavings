using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using BankTechAccountSavings.Application.Transactions.Dtos;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using BankTechAccountSavings.Domain.Entities;

namespace BankTechAccountSavings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountSavingController(IAccountSavingService accountSavingService, IValidator<CreateAccountSaving> createAccountSavingValidator, IValidator<UpdateAccountSaving> updateAccountSavingValidator, IValidator<CreateDeposit> createDepositValidator, IValidator<CreateWithdraw> createWithdrawValidator,IValidator<CreateBankTransfer> createBankTransfer, IValidator<CreateInterBankTransfer> createTransferValidator) : ControllerBase
    {
        private readonly IAccountSavingService _accountService = accountSavingService;
        private readonly IValidator<CreateAccountSaving> _createAccountSavingValidator = createAccountSavingValidator;
        private readonly IValidator<UpdateAccountSaving> _updateAccountSavingValidator = updateAccountSavingValidator;
        private readonly IValidator<CreateDeposit> _createDepositValidator = createDepositValidator;
        private readonly IValidator<CreateWithdraw> _createWithdrawValidator = createWithdrawValidator;
        private readonly IValidator<CreateBankTransfer> _createBankTransferValidator = createBankTransfer;
        private readonly IValidator<CreateInterBankTransfer> _createTransferValidator = createTransferValidator;

        [HttpGet()]
        public async Task<ActionResult<List<GetAccountSaving>>> GetAccounts()
        {
            try
            {
                List<GetAccountSaving> accountSavings = await _accountService.GetAllAccountsAsync();

                if (accountSavings == null || accountSavings.Count == 0)
                {
                    return NotFound(_accountService.FormatErrorResponse("No Accounts were found"));
                }

                return Ok(accountSavings);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpGet("{clientId:int}/paginated")]
        public async Task<ActionResult<Paginated<GetAccountSaving>>> GetPaginatedAccounts(int clientId, int page, int pageSize)
        {
            try
            {
                Paginated<GetAccountSaving> paginatedResult = await _accountService.GetPaginatedAccountsByClientIdAsync(clientId, page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("No Accounts were found"));
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpGet("client/{clientId:int}/paginated/transactions")]
        public async Task<ActionResult<Paginated<GetTransaction>>> GetClientPaginatedTransactions(int clientId, int page, int pageSize)
        {
            try
            {
                Paginated<GetTransaction> paginatedResult = await _accountService.GetPaginatedTransactionsByClientIdAsync(clientId, page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound(_accountService.FormatErrorResponse($"No Transactions were found for the client"));
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpGet("{clientId:int}/paginated/transfers")]
        public async Task<ActionResult<Paginated<GetTransfer>>> GetPaginatedTransfersByAccount(int clientId, int page, int pageSize)
        {
            try
            {
                Paginated<GetTransfer> paginatedResult = await _accountService.GetPaginatedTransfersByClientIdAsync(clientId, page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound(_accountService.FormatErrorResponse($"No Transfers Transaction were found for the client"));
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpGet("{clientId:int}/paginated/beneficiaries")]
        public async Task<ActionResult<Paginated<GetBeneficiary>>> GetPaginatedBeneficiariesByAccount(int clientId, int page, int pageSize)
        {
            try
            {
                Paginated<GetBeneficiary> paginatedResult = await _accountService.GetPaginatedBeneficiariesByClientIdAsync(clientId, page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound(_accountService.FormatErrorResponse($"No Beneficiaries were found for the client"));
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpGet("{accountNumber:long}/paginated/transactions")]
        public async Task<ActionResult<Paginated<GetTransaction>>> GetPaginatedTransactions(long accountNumber, int page, int pageSize)
        {
            try
            {
                Paginated<GetTransaction> paginatedResult = await _accountService.GetPaginatedTransactionsByAccountNumberAsync(accountNumber, page, pageSize);

                if (paginatedResult.Items == null)
                {
                    return NotFound(_accountService.FormatErrorResponse($"No Transactions were found for the account {accountNumber}"));
                }

                return Ok(paginatedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<GetDeposit>?> AddDeposit(CreateDeposit createDeposit)
        {
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createDeposit, _createDepositValidator);
                if (validationResult != null)
                {
                    return validationResult;
                }
                GetDeposit? deposit = await _accountService.AddDepositAsync(createDeposit);

                if (deposit == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("Deposit failed"));
                }

                return Ok(deposit);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpPost("withdraw")]
        public async Task<ActionResult<GetWithdraw>?> AddWithDraw(CreateWithdraw createWithdraw)
        {
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createWithdraw, _createWithdrawValidator);
                if (validationResult != null)
                {
                    return validationResult;
                }
                GetWithdraw? withdraw = await _accountService.CreateWithdrawAsync(createWithdraw);

                if (withdraw == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("WithDraw failed"));
                }

                return Ok(withdraw);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<GetTransfer>?> TransferFunds(CreateBankTransfer createTransfer)
        {
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createTransfer, _createBankTransferValidator);
                if (validationResult != null)
                {
                    return validationResult;
                }
                
                GetTransfer? transaction = await _accountService.CreateBankTransferAsync(createTransfer);

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

        [HttpPost("interbanktransfer")]
        public async Task<ActionResult<GetTransfer>?> InterBankTransferFunds(CreateInterBankTransfer createTransfer)
        {
            try
            {
                ActionResult? validationResult = await ValidateAndReturnResultAsync(createTransfer, _createTransferValidator);
                if (validationResult != null)
                {
                    return validationResult;
                }
                GetTransfer? transaction = await _accountService.CreateInterBankTransferAsync(createTransfer);

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
                    return NotFound(_accountService.FormatErrorResponse("Failed to create the account"));
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
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
                    return NotFound(_accountService.FormatErrorResponse("Failed to update the account"));
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
            }

        }

        [HttpPatch("close")]
        public async Task<ActionResult<DeletedAccountSavingResponse>?> CloseAccount([FromBody] Guid accountId, string reasonToCloseAccount)
        {
            try
            {
                DeletedAccountSavingResponse? account = await _accountService.DeleteAccountSavingAsync(accountId, reasonToCloseAccount);

                if (account == null)
                {
                    return NotFound(_accountService.FormatErrorResponse("Failed to close the account"));
                }

                return Ok("Account is closed");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _accountService.FormatErrorResponse(ex.Message));
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
