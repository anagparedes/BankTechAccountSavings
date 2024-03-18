using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Application.AccountSavings.Dtos;
using BankTechAccountSavings.Application.AccountSavings.Interfaces;
using AutoMapper;
using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Application.Transactions.Dtos;

namespace BankTechAccountSavings.Application.AccountSavings.Services
{
    internal class AccountSavingService(IAccountSavingRepository accountSavingRepository, IMapper mapper) : IAccountSavingService

    {
        private readonly IAccountSavingRepository _accountSavingRepository = accountSavingRepository;
        private readonly IMapper _mapper = mapper;

        async Task<List<GetAccountSaving>> IAccountSavingService.GetAllAccountsAsync()
        {
            List<AccountSaving>? accountSaving = await _accountSavingRepository.GetAllAsync();
            return accountSaving.Select(st => _mapper.Map<GetAccountSaving>(st)).ToList();
        }

        async Task<Paginated<GetAccountSaving>> IAccountSavingService.GetPaginatedAccountsByClientIdAsync(int clientId, int page, int pageSize)
        {
            IQueryable<AccountSaving> queryable = _accountSavingRepository.GetAllQueryable(clientId);
            Paginated<AccountSaving> paginatedResult = await _accountSavingRepository.GetAccountsPaginatedAsync(queryable, page, pageSize);

            List<GetAccountSaving> result = paginatedResult.Items?.Count > 0
                ? paginatedResult.Items.Select(st => _mapper.Map<GetAccountSaving>(st)).ToList() 
                : paginatedResult.CurrentPage > paginatedResult.TotalPages 
                ? throw new InvalidOperationException("There are no more accounts to show") 
                : throw new InvalidOperationException("The client doesn't have accounts");

            return new Paginated<GetAccountSaving>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        async Task<Paginated<GetTransaction>> IAccountSavingService.GetPaginatedTransactionsByClientIdAsync(int ClientId, int page, int pageSize)
        {
            IQueryable<Transaction> queryable = _accountSavingRepository.GetTransactionsByClientQueryable(ClientId);

            Paginated<Transaction> paginatedResult = await _accountSavingRepository.GetTransactionsPaginatedAsync(queryable, page, pageSize);

            List<GetTransaction> result = paginatedResult.Items?.Count > 0
                ? paginatedResult.Items.Select(st => _mapper.Map<GetTransaction>(st)).ToList()
                : paginatedResult.CurrentPage > paginatedResult.TotalPages
                ? throw new InvalidOperationException("There are no more transactions to show")
                : throw new InvalidOperationException("The client doesn't have transactions");

            return new Paginated<GetTransaction>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        async Task<Paginated<GetTransaction>> IAccountSavingService.GetPaginatedTransactionsByAccountNumberAsync(long accountNumber, int page, int pageSize)
        {
            IQueryable<Transaction> queryable = _accountSavingRepository.GetTransactionsByAccountNumberQueryable(accountNumber);

            Paginated<Transaction> paginatedResult = await _accountSavingRepository.GetTransactionsPaginatedAsync(queryable, page, pageSize);

            List<GetTransaction> result = paginatedResult.Items?.Count > 0
                ? paginatedResult.Items.Select(st => _mapper.Map<GetTransaction>(st)).ToList()
                : paginatedResult.CurrentPage > paginatedResult.TotalPages
                ? throw new InvalidOperationException("There are no more transactions to show")
                : throw new InvalidOperationException("The client doesn't have transactions");


            return new Paginated<GetTransaction>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        async Task<CreatedAccountSavingResponse?> IAccountSavingService.CreateAccountSavingAsync(CreateAccountSaving createAccountSaving)
        {
            AccountSaving account = new()
            {
                ClientId = createAccountSaving.ClientId,
                Currency = createAccountSaving.Currency,
            };
            AccountSaving? newAccount = await _accountSavingRepository.CreateAsync(account);

            await _accountSavingRepository.SaveChangesAsync();

            return _mapper.Map<CreatedAccountSavingResponse>(newAccount);
        }

        async Task<GetDeposit?> IAccountSavingService.AddDepositAsync(CreateDeposit createDeposit)
        {
            var deposit = new Deposit
            {
                Amount = createDeposit.Amount,
                DestinationProductNumber = createDeposit.DestinationProductNumber,
                Description = createDeposit.Description,
            };
            Deposit? transaction = await _accountSavingRepository.CreateDepositAsync(deposit);
                await _accountSavingRepository.SaveChangesAsync();
                return _mapper.Map<GetDeposit>(transaction);
        }

        async Task<GetWithdraw?> IAccountSavingService.CreateWithdrawAsync(CreateWithdraw createWithdraw)
        {
            var withDraw = new Withdraw
            {
                Amount = createWithdraw.Amount,
                SourceProductNumber = createWithdraw.SourceProductNumber,
            };
            Withdraw? transaction = await _accountSavingRepository.CreateWithdrawAsync(withDraw);
            await _accountSavingRepository.SaveChangesAsync();
            return _mapper.Map<GetWithdraw>(transaction);
        }

        async Task<GetTransfer?> IAccountSavingService.CreateBankTransferAsync(CreateBankTransfer createTransfer)
        {
            var transfer = new Transfer
            {
                DestinationProductNumber = createTransfer.DestinationProductNumber,
                SourceProductNumber = createTransfer.SourceProductNumber,
                Amount = createTransfer.Amount,
                Description = createTransfer.Description,
                TransferType = createTransfer.TransferType,
            };
            Transfer? transaction = await _accountSavingRepository.CreateBankTransferAsync(transfer);
            await _accountSavingRepository.SaveChangesAsync();
            return _mapper.Map<GetTransfer>(transaction);
        }

        async Task<GetTransfer?> IAccountSavingService.CreateInterBankTransferAsync(CreateInterBankTransfer createTransfer)
        {
            var transfer = new Transfer
            {
                DestinationProductNumber = createTransfer.DestinationProductNumber,
                SourceProductNumber = createTransfer.SourceProductNumber,
                Amount = createTransfer.Amount,
                Description = createTransfer.Description,
                TransferType = createTransfer.TransferType
            };
            Transfer? transaction = await _accountSavingRepository.CreateInterBankTransferAsync(transfer);
            await _accountSavingRepository.SaveChangesAsync();
            return _mapper.Map<GetTransfer>(transaction);
        }

        async Task<UpdatedAccountSavingResponse?> IAccountSavingService.UpdateAccountSavingAsync(Guid accountId, UpdateAccountSaving updateAccountSaving)
        {
            var account = new AccountSaving
            {
                AccountName = updateAccountSaving.AccountName,
                AccountStatus = updateAccountSaving.AccountStatus,
            };
            var newAccount = await _accountSavingRepository.UpdateAsync(accountId, account);
            await _accountSavingRepository.SaveChangesAsync();
            return _mapper.Map<UpdatedAccountSavingResponse>(newAccount);
        }

        async Task<DeletedAccountSavingResponse?> IAccountSavingService.DeleteAccountSavingAsync(Guid accountId, string reasonToCloseAccount)
        {
            AccountSaving? accountSaving = await _accountSavingRepository.DeleteAsync(accountId, reasonToCloseAccount);
            await _accountSavingRepository.SaveChangesAsync();
            return _mapper.Map<DeletedAccountSavingResponse>(accountSaving);
        }

        public string FormatErrorResponse(string errorMessage)
        {
            return $"\"errorMessage\": \"{errorMessage}\"";
        }

        public async Task<Paginated<GetTransfer>> GetPaginatedTransfersByClientIdAsync(int clientId, int page, int pageSize)
        {
            IQueryable<Transfer> queryable = _accountSavingRepository.GetTransfersByClientQueryable(clientId);

            Paginated<Transfer> paginatedResult = await _accountSavingRepository.GetTransfersPaginatedAsync(queryable, page, pageSize);

            List<GetTransfer> result = paginatedResult.Items?.Count > 0
                ? paginatedResult.Items.Select(st => _mapper.Map<GetTransfer>(st)).ToList()
                : paginatedResult.CurrentPage > paginatedResult.TotalPages
                ? throw new InvalidOperationException("There are no more transfer transactions to display")
                : throw new InvalidOperationException("There are no transfer transactions");

            return new Paginated<GetTransfer>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<Paginated<GetBeneficiary>> GetPaginatedBeneficiariesByClientIdAsync(int clientId, int page, int pageSize)
        {
            IQueryable<Beneficiary> queryable = _accountSavingRepository.GetBeneficiariesByClientQueryable(clientId);

            Paginated<Beneficiary> paginatedResult = await _accountSavingRepository.GetBeneficiaryPaginatedAsync(queryable, page, pageSize);

            List<GetBeneficiary> result = paginatedResult.Items?.Count > 0
                ? paginatedResult.Items.Select(st => _mapper.Map<GetBeneficiary>(st)).ToList()
                : paginatedResult.CurrentPage > paginatedResult.TotalPages
                ? throw new InvalidOperationException("There are no more beneficiaries to display")
                : throw new InvalidOperationException("There are no beneficiaries");

            return new Paginated<GetBeneficiary>
            {
                Items = result,
                TotalItems = paginatedResult.TotalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }
    }
}
