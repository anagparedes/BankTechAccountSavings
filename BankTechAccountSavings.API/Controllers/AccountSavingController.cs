using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.AccountSavings.Exceptions;
using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankTechAccountSavings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountSavingController(IAccountSavingService accountSavingService) : ControllerBase
    {
        private readonly IAccountSavingService _accountService = accountSavingService;

        [HttpGet("/GetAccounts")]
        public async Task<ActionResult<List<GetAccountSaving>>> GetAccounts()
        {
            try
            {
                return Ok(await _accountService.GetAllAccountsAsync());
            }
            catch (AccountSavingListNotFound ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("/GetAccountById/{id:Guid}")]
        public async Task<ActionResult<GetAccountSaving>> GetAccountById(Guid id)
        {
            try
            {
                return Ok(await _accountService.GetAccountSavingByIdAsync(id));
            }
            catch (AccountSavingNotFound ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("/GetAccountByAccountNumber/{accountNumber:int}")]
        public async Task<ActionResult<GetAccountSaving>> GetAccountByAccountNumber(int accountNumber)
        {
            try
            {
                return Ok(await _accountService.GetAccountSavingByAccountNumberAsync(accountNumber));
            }
            catch (AccountSavingNotFound ex)
            {
                return NotFound(ex.Message);
            }

        }

        /*[HttpGet("/GetBalance")]
        public async Task<ActionResult<GetAccountSaving>> GetCurrentBalance(int accountNumber)
        {
            try
            {
                //return Ok(await _accountService.GetCurrentBalanceAsync(accountNumber));
            }
            catch (AccountSavingNotFound ex)
            {
                return NotFound(ex.Message);
            }

        }*/

    }
}
