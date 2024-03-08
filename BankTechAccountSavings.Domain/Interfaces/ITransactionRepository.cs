using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction?> GetbyIdAsync(Guid id);
        Task<Transaction?> DeleteAsync(Guid id);
        Task<Transaction?> TransferFunds(Guid fromAccountId, Guid toAccountId, int transferAmount, TransactionType transactionType);
    }
}
