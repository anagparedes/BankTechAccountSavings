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
        Task<List<Transfer>> GetAllAsync();
        Task<Transfer?> GetbyIdAsync(Guid transactionId);
    }
}
