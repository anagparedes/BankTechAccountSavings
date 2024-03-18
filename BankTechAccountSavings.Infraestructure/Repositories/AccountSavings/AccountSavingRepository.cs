using BankTechAccountSavings.Domain.Entities;
using BankTechAccountSavings.Domain.Enums;
using BankTechAccountSavings.Domain.Interfaces;
using BankTechAccountSavings.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;

namespace BankTechAccountSavings.Infraestructure.Repositories.AccountSavings
{
    internal class AccountSavingRepository(AccountSavingDbContext context) : IAccountSavingRepository
    {
        private readonly AccountSavingDbContext _context = context;
        private readonly Random _random = new();
        private readonly DateTimeOffset _current_date = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(-4));

        public async Task<AccountSaving?> CreateAsync(AccountSaving entity, CancellationToken cancellationToken)
        {
            entity.AccountNumber = GenerateBankAccountNumber();

            while (await _context.Set<AccountSaving>().AnyAsync(e => e.AccountNumber == entity.AccountNumber, cancellationToken))
            {
                entity.AccountNumber = GenerateBankAccountNumber();
            }
            entity.AccountType = "Ahorro";
            entity.AccountName = $"CTA.{entity.AccountType}";
            entity.Bank = "BankTech";
            entity.CurrentBalance = 0;
            entity.Currency = entity.Currency != 0 ? entity.Currency : Currency.DOP;
            entity.AccountStatus = AccountStatus.Pendiente;
            entity.IsActive = false;

            await _context.Set<AccountSaving>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<Paginated<Beneficiary>> GetBeneficiaryPaginatedAsync(IQueryable<Beneficiary> queryable, int page, int pageSize)
        {
            var totalItems = await queryable.CountAsync();

            var paginatedItems = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginated<Beneficiary>
            {
                Items = paginatedItems,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }
        public IQueryable<Beneficiary> GetBeneficiariesByClientQueryable(int clientId)
        {
            IQueryable<Beneficiary> beneficiaries = _context.Beneficiaries
                .Where(b => b.AccountSavingAssociate!.ClientId == clientId)
                .OrderByDescending(b => b.CreatedDate);

            if (beneficiaries.Any() == false)
            {
                throw new InvalidOperationException("The client doesn't has beneficiaries");
            }
            return beneficiaries;
        }
        public async Task<AccountSaving?> DeleteAsync(Guid accountId, string reasonToCloseAccount, CancellationToken cancellationToken)
        {
            AccountSaving account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId, cancellationToken) ?? throw new InvalidOperationException($"The Account is not found");

            account.AccountStatus = AccountStatus.Cerrada;
            _context.Set<AccountSaving>().Remove(account);

            return account;
        }

        public async Task<List<AccountSaving>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<AccountSaving>().Where(x => !x.IsDeleted).ToListAsync(cancellationToken);
        }
        public async Task<Paginated<AccountSaving>> GetAccountsPaginatedAsync(IQueryable<AccountSaving> queryable, int page, int pageSize)
        {
            var totalItems = await queryable.CountAsync();

            var paginatedItems = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginated<AccountSaving>
            {
                Items = paginatedItems,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<Paginated<Transaction>> GetTransactionsPaginatedAsync(IQueryable<Transaction> queryable, int page, int pageSize)
        {
            var totalItems = await queryable.CountAsync();

            var paginatedItems = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginated<Transaction>
            {
                Items = paginatedItems,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public async Task<AccountSaving?> GetbyIdAsync(Guid accountId, CancellationToken cancellationToken)
        {
            AccountSaving account = await _context.Set<AccountSaving>().FindAsync(new object[] { accountId }, cancellationToken) ?? throw new InvalidOperationException($"The Account is not found");
            return account;
        }

        public async Task<AccountSaving?> UpdateAsync(Guid accountId, AccountSaving entity, CancellationToken cancellationToken)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId, cancellationToken) ?? throw new InvalidOperationException($"The Account is not found");
            if (entity.AccountName != "string")
            {
                account.AccountName = entity.AccountName;
            }

            account.AccountStatus = entity.AccountStatus != 0 ? entity.AccountStatus : account.AccountStatus;
            await CalculateAndResetInterest(account.Id, cancellationToken);

            return account;
        }

        public async Task CalculateAndResetInterest(Guid accountId, CancellationToken cancellationToken)
        {
            if (await HasReachedEndOfMonth(accountId))
            {
                AccountSaving? account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId, cancellationToken) ?? throw new InvalidOperationException($"The Account is not found");
                ;
                CalculateInterest(accountId);
                account.MonthlyInterestGenerated = 0;
            }
        }

        private async void CalculateInterest(Guid accountId)
        {
            AccountSaving? account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId);

            if (account == null) return;

            decimal monthlyInterestRate = account.AnnualInterestRate / 12;

            account.MonthlyInterestGenerated = account.CurrentBalance * monthlyInterestRate;
            account.AnnualInterestProjected += account.MonthlyInterestGenerated * 12;
        }
        private async ValueTask<bool> HasReachedEndOfMonth(Guid accountId)
        {
            AccountSaving? account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.Id == accountId);

            return _current_date.Month != account?.DateOpened.Month || _current_date.Year != account.DateOpened.Year;
        }

        private long GenerateBankAccountNumber()
        {
            long minAccountNumber = 1000000000;
            long maxAccountNumber = 9999999999;
            long accountNumber = (long)(_random.NextDouble() * (maxAccountNumber - minAccountNumber) + minAccountNumber);

            return accountNumber;
        }

        private long GenerateWithdrawPassword()
        {
            long minAccountNumber = 10000000;
            long maxAccountNumber = 99999999;
            long accountNumber = (long)(_random.NextDouble() * (maxAccountNumber - minAccountNumber) + minAccountNumber);
            return accountNumber;
        }

        private long GenerateWithdrawCode()
        {
            long minAccountNumber = 1000;
            long maxAccountNumber = 9999;

            long accountNumber = (long)(_random.NextDouble() * (maxAccountNumber - minAccountNumber) + minAccountNumber);
            return accountNumber;
        }

        private int GenerateConfirmationNumber()
        {

            int minConfirmationNumber = 1000000;
            int maxConfirmationNumber = 9999999;

            int confirmationNumber = _random.Next(minConfirmationNumber, maxConfirmationNumber + 1);

            return confirmationNumber;
        }
        private long GenerateVoucherNumber()
        {
            long minConfirmationNumber = 1000000000;
            long maxConfirmationNumber = 9999999999;

            long confirmationNumber = (long)(_random.NextDouble() * (maxConfirmationNumber - minConfirmationNumber) + minConfirmationNumber);

            return confirmationNumber;
        }

        public IQueryable<AccountSaving> GetAllQueryable(int clientId)
        {
            IQueryable<AccountSaving> accounts = _context.Set<AccountSaving>().Where(acc => acc.ClientId == clientId && !acc.IsDeleted)
                .OrderByDescending(t => t.CreatedDate);
            if (accounts.Any() == false)
            {
                throw new InvalidOperationException("The client doesn't has accounts");
            }
            return accounts;
        }

        public IQueryable<Transaction> GetTransactionsByClientQueryable(int clientId)
        {
            IQueryable<Transaction> transactions = _context.Transactions.Where(t =>
            (t is Deposit && ((Deposit)t).DestinationProduct!.ClientId == clientId) ||
            (t is Transfer && (((Transfer)t).SourceProduct!.ClientId == clientId ||
            ((Transfer)t).DestinationProduct!.ClientId == clientId)) ||
            (t is Withdraw && ((Withdraw)t).SourceProduct!.ClientId == clientId)
            )
                .OrderByDescending(t => t.CreatedDate);
            return transactions;
        }

        public IQueryable<Transaction> GetTransactionsByAccountNumberQueryable(long accountNumber)
        {
            IQueryable<Transaction> transactions = _context.Transactions.Where(t =>
            (t is Deposit && ((Deposit)t).DestinationProductNumber == accountNumber) ||
            (t is Transfer && (((Transfer)t).SourceProductNumber == accountNumber ||
            ((Transfer)t).DestinationProductNumber == accountNumber)) ||
            (t is Withdraw && ((Withdraw)t).SourceProductNumber == accountNumber)
           )
        .OrderByDescending(t => t.CreatedDate);

            return transactions;
        }

        public async Task<Deposit?> CreateDepositAsync(Deposit entity, CancellationToken cancellationToken = default)
        {
            AccountSaving? account = await _context.AccountSavings.FirstOrDefaultAsync(s => s.AccountNumber == entity.DestinationProductNumber, cancellationToken);
            Beneficiary? beneficiary = await _context.Beneficiaries.FirstOrDefaultAsync(s => s.BeneficiaryAccountNumber == entity.DestinationProductNumber, cancellationToken);

            if (entity.Amount <= 0)
            {
                throw new InvalidOperationException("Invalid Deposit Amount");
            }
            if (account is not null)
            {
                if (account!.IsActive == false)
                {
                    Deposit deposit = CheckAndUpdateAccountStatus(account, entity);

                    await _context.Set<Deposit>().AddAsync(deposit, cancellationToken);
                    return account.Deposits.LastOrDefault();

                }
                if (account.Currency != entity.Currency)
                {
                    switch (account.Currency)
                    {
                        case Currency.DOP:
                            decimal convertedAmount = ConvertToDominicanPeso(entity.Amount);
                            entity.Amount = convertedAmount;
                            break;
                        case Currency.USD:
                            decimal convertedUSDAmount = ConvertToUSD(entity.Amount);
                            entity.Amount = convertedUSDAmount;
                            break;
                        default:
                            throw new InvalidOperationException("Moneda Inválida");

                    }
                }


                account.CurrentBalance += entity.Amount;

                Deposit newDeposit = new()
                {
                    DestinationProduct = account,
                    DestinationProductId = account.Id,
                    DestinationProductNumber = account.AccountNumber,
                    TransactionDate = DateTime.UtcNow.Date.ToLocalTime(),
                    ConfirmationNumber = GenerateConfirmationNumber(),
                    Voucher = GenerateVoucherNumber(),
                    Description = entity.Description,
                    TransactionType = TransactionType.Deposit,
                    TransactionStatus = TransactionStatus.Completed,
                    Amount = entity.Amount,
                };
                account.Deposits.Add(newDeposit);
                await _context.Set<Deposit>().AddAsync(newDeposit, cancellationToken);

                return account.Deposits.LastOrDefault();

            }
            else if(beneficiary is not null)
            {
                if (beneficiary.Currency != entity.Currency)
                {
                    switch (beneficiary.Currency)
                    {
                        case Currency.DOP:
                            decimal convertedAmount = ConvertToDominicanPeso(entity.Amount);
                            entity.Amount = convertedAmount;
                            break;
                        case Currency.USD:
                            decimal convertedUSDAmount = ConvertToUSD(entity.Amount);
                            entity.Amount = convertedUSDAmount;
                            break;
                        default:
                            throw new InvalidOperationException("Moneda Inválida");

                    }
                }


                //beneficiary.CurrentBalance += entity.Amount;

                Deposit newDeposit = new()
                {
                    DestinationProductNumber = beneficiary.BeneficiaryAccountNumber,
                    TransactionDate = DateTime.UtcNow.Date.ToLocalTime(),
                    ConfirmationNumber = GenerateConfirmationNumber(),
                    Voucher = GenerateVoucherNumber(),
                    Description = entity.Description,
                    TransactionType = TransactionType.Deposit,
                    TransactionStatus = TransactionStatus.Completed,
                    Amount = entity.Amount,
                };
                //beneficiary.Deposits.Add(newDeposit);
                //TODO: Tengo que guardarlo en base de datos (?)
                //await _context.Set<Deposit>().AddAsync(newDeposit, cancellationToken);

                return newDeposit;
            }
            throw new InvalidOperationException("The account does not exist.");
        }

        async Task<Transfer?> IAccountSavingRepository.CreateBankTransferAsync(Transfer entity, CancellationToken cancellationToken)
        {
            AccountSaving? fromAccount = await _context.Set<AccountSaving>()
     .FirstOrDefaultAsync(s => s.AccountNumber == entity.SourceProductNumber, cancellationToken) ?? throw new InvalidOperationException($"The Source Account was not found. Please ensure that the source account exists and is correctly specified.");

            if (fromAccount.IsActive == false)
            {
                throw new InvalidOperationException($"El Producto de Origen no está activado.");
            }

            AccountSaving? toAccount = await _context.Set<AccountSaving>()
                .FirstOrDefaultAsync(s => s.AccountNumber == entity.DestinationProductNumber, cancellationToken) ?? throw new InvalidOperationException($"The Destination Account was not found. Please ensure that the destination account exists and is correctly specified.");

            if (toAccount.IsActive == false)
            {
                throw new InvalidOperationException($"El Producto de Destino no está activado.");
            }

            if (entity.Amount <= 0)
            {
                throw new InvalidOperationException("Invalid transfer amount: Amount must be greater than zero");
            }

            if (entity.Amount > fromAccount.CurrentBalance)
            {
                throw new InvalidOperationException("Insufficient funds: The transfer amount exceeds the current balance");
            }
            if (fromAccount.Currency != toAccount.Currency)
            {
                switch (toAccount.Currency)
                {
                    case Currency.DOP:
                        decimal convertedAmount = ConvertToDominicanPeso(entity.Amount);
                        entity.Amount = convertedAmount;
                        break;
                    case Currency.USD:
                        decimal convertedUSDAmount = ConvertToUSD(entity.Amount);
                        entity.Amount = convertedUSDAmount;
                        break;
                    default:
                        throw new InvalidOperationException("Moneda Inválida");

                }
            }

            if (entity.TransferType == TransferType.Propia)
            {
                if (fromAccount.ClientId != toAccount.ClientId)
                {
                    throw new InvalidOperationException("No tienes productos para está transacción");
                }
                Transfer newTransaction = new()
                {
                    SourceProduct = fromAccount,
                    SourceProductId = fromAccount.Id,
                    SourceProductNumber = fromAccount.AccountNumber,
                    DestinationProduct = toAccount,
                    DestinationProductId = toAccount.Id,
                    DestinationProductNumber = toAccount.AccountNumber,
                    TransferType = entity.TransferType,
                    TransactionType = TransactionType.Transfer,
                    InterbankTransferType = InterbankTransferType.LBTR,
                    Description = entity.Description,
                    TransactionDate = _current_date,
                    ConfirmationNumber = GenerateConfirmationNumber(),
                    Voucher = GenerateVoucherNumber(),
                    TransactionStatus = TransactionStatus.Completed,
                    Commission = 0,
                    Amount = entity.Amount,
                    Tax = 0.0015m * entity.Amount,
                    Total = (entity.Amount + entity.Commission + (0.0015m * entity.Amount)),
                    Credit = entity.Amount,
                    Debit = entity.Amount,
                };

                fromAccount.CurrentBalance -= newTransaction.Total;
                toAccount.CurrentBalance += entity.Amount;

                fromAccount.TransfersAsSource.Add(newTransaction);
                toAccount.TransfersAsDestination.Add(newTransaction);

                return newTransaction;

            }
            else if (entity.TransferType == TransferType.Tercero)
            {
                if (fromAccount.ClientId == toAccount.ClientId)
                {
                    throw new InvalidOperationException("Los productos no pueden pertenecer al mismo cliente para esta operación.");
                }
                Transfer newTransaction = new()
                {
                    SourceProduct = fromAccount,
                    SourceProductId = fromAccount.Id,
                    SourceProductNumber = fromAccount.AccountNumber,
                    DestinationProduct = toAccount,
                    DestinationProductId = toAccount.Id,
                    DestinationProductNumber = toAccount.AccountNumber,
                    TransferType = entity.TransferType,
                    TransactionType = TransactionType.Transfer,
                    InterbankTransferType = InterbankTransferType.LBTR,
                    Description = entity.Description,
                    TransactionDate = _current_date,
                    ConfirmationNumber = GenerateConfirmationNumber(),
                    Voucher = GenerateVoucherNumber(),
                    TransactionStatus = TransactionStatus.Completed,
                    Amount = entity.Amount,
                    Commission = 0,
                    Tax = 0.0015m * entity.Amount,
                    Total = (entity.Amount + entity.Commission + (0.0015m * entity.Amount)),
                    Credit = entity.Amount,
                    Debit = entity.Amount,
                };

                fromAccount.CurrentBalance -= newTransaction.Total;
                toAccount.CurrentBalance += entity.Amount;

                fromAccount.TransfersAsSource.Add(newTransaction);
                toAccount.TransfersAsDestination.Add(newTransaction);

                return newTransaction;
            }
            throw new InvalidOperationException("La transferencia no pudo ser completada, intentelo más tarde.");

        }

        async Task<Transfer?> IAccountSavingRepository.CreateInterBankTransferAsync(Transfer entity, CancellationToken cancellationToken)
        {
            AccountSaving? fromAccount = await _context.Set<AccountSaving>()
     .FirstOrDefaultAsync(s => s.AccountNumber == entity.SourceProductNumber, cancellationToken) ?? throw new InvalidOperationException($"The Source Account was not found. Please ensure that the source account exists and is correctly specified.");

            if (fromAccount.IsActive == false)
            {
                throw new InvalidOperationException($"The Source Account is not active.");
            }

            Beneficiary? toAccount = await _context.Set<Beneficiary>()
                .FirstOrDefaultAsync(s => s.BeneficiaryAccountNumber == entity.DestinationProductNumber, cancellationToken) ?? throw new InvalidOperationException($"The Destination Account was not found. Please ensure that the destination account exists and is correctly specified.");

            if (entity.Amount <= 0)
            {
                throw new InvalidOperationException("Invalid transfer amount: Amount must be greater than zero");
            }

            if (entity.Amount > fromAccount.CurrentBalance)
            {
                throw new InvalidOperationException("Insufficient funds: The transfer amount exceeds the current balance");
            }
            //TODO: verificar de que funcione
            int commission = (entity.InterbankTransferType == InterbankTransferType.LBTR) ? 100 : 0;

            if (entity.TransferType == TransferType.Tercero)
            {
                if (fromAccount.ClientId == toAccount.ClientId)
                {
                    throw new InvalidOperationException("Los productos no pueden pertenecer al mismo cliente para esta operación.");
                }
                if (fromAccount.Currency != toAccount.Currency)
                {
                    switch (toAccount.Currency)
                    {
                        case Currency.DOP:
                            decimal convertedAmount = ConvertToDominicanPeso(entity.Amount);
                            entity.Amount = convertedAmount;
                            break;
                        case Currency.USD:
                            decimal convertedUSDAmount = ConvertToUSD(entity.Amount);
                            entity.Amount = convertedUSDAmount;
                            break;
                        default:
                            throw new InvalidOperationException("Moneda Inválida");

                    }
                }
                Transfer newTransaction = new()
                {
                    SourceProduct = fromAccount,
                    SourceProductId = fromAccount.Id,
                    SourceProductNumber = fromAccount.AccountNumber,
                    //DestinationProduct = toAccount.BeneficiaryAccountSaving,
                    //DestinationProductId = toAccount.BeneficiaryAccountSaving!.Id,
                    DestinationProductNumber = toAccount.BeneficiaryAccountNumber,
                    TransferType = TransferType.Tercero,
                    InterbankTransferType = entity.InterbankTransferType,
                    TransactionType = TransactionType.Transfer,
                    Description = entity.Description,
                    TransactionDate = _current_date,
                    ConfirmationNumber = GenerateConfirmationNumber(),
                    Voucher = GenerateVoucherNumber(),
                    TransactionStatus = TransactionStatus.Completed,
                    Amount = entity.Amount,
                    Tax = 0.0015m * entity.Amount,
                    Total = (entity.Amount + commission + (0.0015m * entity.Amount)),
                    Credit = entity.Amount,
                    Debit = entity.Amount,
                };
                fromAccount.CurrentBalance -= newTransaction.Total;
                //toAccount.CurrentBalance += entity.Amount;

                fromAccount.TransfersAsSource.Add(newTransaction);

                return newTransaction;
            }
            else if (entity.TransferType == TransferType.Propia)
            {
                if (fromAccount.ClientId != toAccount.ClientId)
                {
                    throw new InvalidOperationException("No tienes productos para está transacción");
                }
                Transfer newTransaction = new()
                {
                    SourceProduct = fromAccount,
                    SourceProductId = fromAccount.Id,
                    SourceProductNumber = fromAccount.AccountNumber,
                    //DestinationProduct = toAccount.BeneficiaryAccountSaving,
                    //DestinationProductId = toAccount.BeneficiaryAccountSaving!.Id,
                    DestinationProductNumber = toAccount.BeneficiaryAccountNumber,
                    TransferType = TransferType.Propia,
                    TransactionType = TransactionType.Transfer,
                    InterbankTransferType = entity.InterbankTransferType,
                    Description = entity.Description,
                    TransactionDate = _current_date,
                    ConfirmationNumber = GenerateConfirmationNumber(),
                    Voucher = GenerateVoucherNumber(),
                    TransactionStatus = TransactionStatus.Completed,
                    Amount = entity.Amount,
                    Tax = 0.0015m * entity.Amount,
                    Total = (entity.Amount + commission + (0.0015m * entity.Amount)),
                    Credit = entity.Amount,
                    Debit = entity.Amount,
                };

                fromAccount.CurrentBalance -= newTransaction.Total;
                fromAccount.TransfersAsSource.Add(newTransaction);

                return newTransaction;
            }
            throw new InvalidOperationException("La transferencia no pudo ser completada, intentelo más tarde.");
        }

        async Task<Withdraw?> IAccountSavingRepository.CreateWithdrawAsync(Withdraw entity, CancellationToken cancellationToken)
        {
            var account = await _context.Set<AccountSaving>().FirstOrDefaultAsync(s => s.AccountNumber == entity.SourceProductNumber, cancellationToken) ?? throw new InvalidOperationException($"The Account not found. Please ensure that the account exists and is correctly specified.\r\n");
            if (account.IsActive == false)
            {
                throw new InvalidOperationException("Esta cuenta no está activada, no tiene permitido realizar está operación");
            }
            if (account.CurrentBalance < entity.Amount)
            {
                throw new InvalidOperationException("Fondos Insuficientes");
            }
            else if (entity.Amount <= 0)
            {
                throw new InvalidOperationException("Monto de retiro inválido");
            }

            Withdraw newWithdraw = new()
            {
                AccountName = account.AccountName,
                SourceProduct = account,
                SourceProductId = account.Id,
                SourceProductNumber = account.AccountNumber,
                WithdrawPassword = GenerateWithdrawPassword(),
                WithdrawCode = GenerateWithdrawCode(),
                Debit = entity.Amount,
                Amount = entity.Amount,
                TransactionDate = DateTime.UtcNow.Date.ToLocalTime(),
                ConfirmationNumber = GenerateConfirmationNumber(),
                Voucher = GenerateVoucherNumber(),
                Description = $"Withdraw on day: {DateTime.UtcNow.Date.ToLocalTime()}",
                TransactionType = TransactionType.WithDraw,
                TransactionStatus = TransactionStatus.Completed,
                Tax = 0.0015m * entity.Amount,
                Total = (entity.Amount + (0.0015m * entity.Amount))
            };
            account.CurrentBalance -= newWithdraw.Total;
            await _context.Set<Withdraw>().AddAsync(newWithdraw, cancellationToken);
            account.WithDraws.Add(newWithdraw);
            return account.WithDraws.LastOrDefault();
        }

        public IQueryable<Transfer> GetTransfersByClientQueryable(int clientId)
        {
            return _context.Transfers.Where(w => w.SourceProduct!.ClientId == clientId || w.DestinationProduct!.ClientId == clientId)
                .OrderByDescending(t => t.TransactionDate);
        }

        public async Task<Paginated<Transfer>> GetTransfersPaginatedAsync(IQueryable<Transfer> queryable, int page, int pageSize)
        {
            var totalItems = await queryable.CountAsync();

            var paginatedItems = await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new Paginated<Transfer>
            {
                Items = paginatedItems,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        private Deposit CheckAndUpdateAccountStatus(AccountSaving account, Deposit deposit)
        {
            TimeSpan timeRemaining = GetRemainingTimeBeforeClosure(account);

            if (timeRemaining.TotalHours > 24 && account.CurrentBalance == 0)
            {
                account.AccountStatus = AccountStatus.Cerrada;
                account.IsActive = false;
                account.DateClosed = _current_date;
                throw new InvalidOperationException($"Su cuenta {account.AccountNumber} ha sido cerrada por no haber realizado el depósito inicial en el plazo acordado.");
            }
            else if (timeRemaining.TotalHours < 24 && account.CurrentBalance == 0)
            {
                if (deposit.Amount < 500)
                {
                    throw new InvalidOperationException($"El monto mínimo de apertura de su cuenta debe de ser de {account.Currency}$500");

                }
                account.CurrentBalance += deposit.Amount;
                account.AnnualInterestRate = 0.30m / 100;
                account.MonthlyInterestGenerated = 0;
                account.DateOpened = _current_date;
                account.AccountStatus = AccountStatus.Activa;
                account.IsActive = true;
                Beneficiary firstBeneficiary = new()
                {
                    AccountSavingAssociate = account,
                    AccountNumberAssociate = account.AccountNumber,
                    AccountId = account.Id,
                    ClientId = account.ClientId,
                    BeneficiaryAccountNumber = 00123456789012345678,
                    Currency = Currency.DOP,
                    Bank = Bank.BanReservas,
                    IdentificationCard = "001-1234567-8",
                    BeneficiaryName = "Juan",
                    BeneficiaryLastName = "Díaz",
                    Email = "juandiaz@example.com",
                };
                Beneficiary secondBeneficiary = new()
                {
                    AccountSavingAssociate = account,
                    AccountNumberAssociate = account.AccountNumber,
                    AccountId = account.Id,
                    ClientId = 1,
                    BeneficiaryAccountNumber = 0301234567890,
                    Currency = Currency.USD,
                    Bank = Bank.BancoBHDLeon,
                    IdentificationCard = "001-7654321-4",
                    BeneficiaryName = "María",
                    BeneficiaryLastName = "Rodríguez",
                    Email = "mrodriguez@example.com"
                };

                Beneficiary thirdBeneficiary = new()
                {
                    AccountSavingAssociate = account,
                    AccountNumberAssociate = account.AccountNumber,
                    AccountId = account.Id,
                    ClientId = 2,
                    BeneficiaryAccountNumber = 54317390275,
                    Currency = Currency.USD,
                    Bank = Bank.Scotiabank,
                    IdentificationCard = "001-9644321-7",
                    BeneficiaryName = " Miguel",
                    BeneficiaryLastName = "Castillo",
                    Email = "mcastillo@example.com"
                };

                Beneficiary fourthBeneficiary = new()
                {
                    AccountSavingAssociate = account,
                    AccountNumberAssociate = account.AccountNumber,
                    AccountId = account.Id,
                    ClientId = 3,
                    BeneficiaryAccountNumber = 64317390275,
                    Currency = Currency.DOP,
                    Bank = Bank.BancoSantaCruz,
                    IdentificationCard = "001-7654321-5",
                    BeneficiaryName = "Laura",
                    BeneficiaryLastName = "García",
                    Email = "lgarcia@example.com"
                };

                /*Beneficiary fifthBeneficiary = new()
                {
                    AccountSavingAssociate = account,
                    AccountNumberAssociate = account.AccountNumber,
                    AccountId = account.Id,
                    ClientId = account.ClientId,
                    BeneficiaryAccountNumber = 00123456789012345675,
                    Currency = Currency.DOP,
                    Bank = Bank.BanReservas,
                    IdentificationCard = "001-0644278-3",
                    BeneficiaryName = "John",
                    BeneficiaryLastName = "Doe",
                    Email = "jdoe@example.com"
                };
*/
                account.Beneficiaries.Add(firstBeneficiary);
                account.Beneficiaries.Add(secondBeneficiary);
                account.Beneficiaries.Add(thirdBeneficiary);
                account.Beneficiaries.Add(fourthBeneficiary);
                //account.Beneficiaries.Add(fifthBeneficiary);


                Deposit newTransaction = new()
                {
                    DestinationProduct = account,
                    DestinationProductId = account.Id,
                    DestinationProductNumber = account.AccountNumber,
                    TransactionDate = _current_date,
                    ConfirmationNumber = GenerateConfirmationNumber(),
                    Voucher = GenerateVoucherNumber(),
                    Description = deposit.Description,
                    TransactionType = TransactionType.Deposit,
                    TransactionStatus = TransactionStatus.Completed,
                    Amount = deposit.Amount,
                };
                account.Deposits.Add(newTransaction);
                return newTransaction;
            }
            else
            {
                throw new InvalidOperationException("El Estado de Cuenta no puede ser actualizado.");
            }
        }
        private static decimal ConvertToDominicanPeso(decimal amountInUSD)
        {
            decimal conversionRate = 59.25m;
            decimal amountConverted = amountInUSD * conversionRate;
            return amountConverted;
        }

        private static decimal ConvertToUSD(decimal amountInDOP)
        {
            decimal conversionRate = 1m / 59.25m;
            decimal amountConverted = amountInDOP * conversionRate;
            return amountConverted;
        }
        private TimeSpan GetRemainingTimeBeforeClosure(AccountSaving account)
        {
            TimeSpan timeElapsed = _current_date - account.CreatedDate;

            TimeSpan timeRemaining = TimeSpan.FromHours(24) - timeElapsed;

            return timeRemaining;
        }
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
