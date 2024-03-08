using AutoMapper;
using BankTechAccountSavings.Application.Transactions.Dtos;
using BankTechAccountSavings.Application.Transactions.Interfaces;
using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Interfaces;

namespace BankTechAccountSavings.Application.Transactions.Services
{
    public class TransactionService(ITransactionRepository transactionRepository, IMapper mapper) : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<GetTransaction>> GetAllAsync()
        {
            List<Transfer> transactions = await _transactionRepository.GetAllAsync();
            return transactions.Select(tr => _mapper.Map<GetTransaction>(tr)).ToList();
        }

        public async Task<GetTransaction?> GetbyIdAsync(Guid id)
        {
            Transfer? transaction = await _transactionRepository.GetbyIdAsync(id);
            return _mapper.Map<GetTransaction>(transaction);
        }
    }
}
