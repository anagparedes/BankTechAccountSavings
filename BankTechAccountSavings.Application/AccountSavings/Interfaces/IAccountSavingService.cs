using BankTechAccountSavings.Application.AccountSavings.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Application.AccountSavings.Interfaces
{
    public interface IAccountSavingService
    {
        Task<List<GetAccountSaving>> GetAllAccountsAsync();
        Task<CreatedAccountSavingResponse?> CreateAccountSavingAsync(CreateAccountSaving createAccountSaving);
        Task<CreatedAccountSavingResponse?> AddDepositAsync(int money, Guid id);
        Task<CreatedAccountSavingResponse?> WithDrawAsync(int accountNumber, int money);
        Task<CreatedAccountSavingResponse?> CloseAccountSavingAsync(int accountNumber);
        Task<GetAccountSaving?> GetAccountSavingByIdAsync(Guid id);
        Task<GetAccountSaving?> GetAccountSavingByAccountNumberAsync(int accountNumber);
        Task<UpdatedAccountSavingResponse?> UpdateAccountSavingBalanceAsync(int id, CreateAccountSaving updateAccountSaving);
        Task<UpdatedAccountSavingResponse?> UpdateAccountSavingStatusAsync(int id, CreateAccountSaving updateAccountSaving);
        Task<DeletedAccountSavingResponse?> DeleteAccountSavingAsync(Guid id);
    }
}
