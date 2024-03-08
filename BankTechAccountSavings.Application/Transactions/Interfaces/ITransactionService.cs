using BankTechAccountSavings.Application.Transactions.Dtos;

namespace BankTechAccountSavings.Application.Transactions.Interfaces
{
    public interface ITransactionService
    {
        Task<List<GetTransaction>> GetAllAsync();
        Task<GetTransaction?> GetbyIdAsync(Guid id);
    }
}
