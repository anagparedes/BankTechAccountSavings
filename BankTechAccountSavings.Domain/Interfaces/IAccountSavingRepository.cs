using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Domain.Interfaces
{
    public interface IAccountSavingRepository: IRepository<AccountSaving>
    {
        Task<AccountSaving?> GetAccountbyAccountNumber(long accountNumber);
        Task<AccountSaving?> AddDepositAsync(int amount, Guid id, string description, TransactionType transactionType);
        Task<AccountSaving?> WithDrawAsync(int amount, Guid id);
        Task<AccountSaving?> CloseAccountSavingAsync(Guid id);


    }
}
