# BankTechAccountSavings

BankTechAccountSavings is a microservice built using .NET Core Web API in .NET 8, focusing on account savings related operations.

## Table of Contents

1. [Requirements](#Requirements)
1. [Getting Started](#getting-started)
1. [Project Structure](#project-structure)
1. [API Documentation](#api-documentation)
1. [Sensitive Information](#sensitive-information)

## Requirements

1. **.NET Core SDK 8.0 or later**: Since the project is built using .NET Core Web API in .NET 8, you'll need to have the .NET Core SDK 8.0 or a later version installed on your machine. You can download the .NET Core SDK from the official website: https://dotnet.microsoft.com/download

2. **Visual Studio IDE (recommended)**: While not strictly required, recommend using Visual Studio IDE if you're not familiar with the command-line interface. Visual Studio IDE provides a user-friendly environment for developing, building, and running .NET applications.

3. **Git**: The project is hosted on a Git repository, so you'll need Git installed to clone the repository. You can download Git from the official website: https://git-scm.com/downloads

4. **Database Connection String**: The project uses User Secrets to store sensitive information, such as the database connection string. You'll need to set up the connection string using the `dotnet user-secrets` command, as mentioned in the "Sensitive Information" section of the documentation.

5. **Database Requirements**: Since the project uses Entity Framework Core, it should be compatible with various database systems supported by EF Core, such as SQL Server, PostgreSQL, or SQLite.

## Getting Started

1. Clone the repository: `git clone https://github.com/anagparedes/BankTechAccountSavings.git`
   **Recommendation: Use Visual Studio IDE if you are not familiar with this**

2. Navigate to the project folder:

    ```bash
    cd BankTechAccountSavings
    ```

3. Build the solution:

    ```bash
    dotnet build
    ```

4. Run the application:

    ```bash
    dotnet run --project BankTechAccountSavings.API
    ```

## Project Structure

- **AccountSaving**
  - **BankTechAccountSavings.API**
    - **Config**
      - DbConfig.cs
    - **Controllers**
      - AccountSavingController.cs
      - TransactionController.cs
  - **BankTechAccountSavings.Application**
    - **AccountSavings**
      - **AutoMapper**
      - **Dtos**
      - **Interfaces**
      - **Services**
      - **Validators**
    - **Transactions**
      - **AutoMapper**
      - **Dtos**
      - **Interfaces**
      - **Services**
      - **Validators**
    - IoC.cs
  - **BankTechAccountSavings.Domain**
    - **Entities**
      - AccountSavings.cs
      - Deposit.cs
      - Paginated.cs
      - Transaction.cs
      - Transfer.cs
      - Withdraw.cs
    - **Enums**
      - AccountStatus.cs
      - Currency.cs
      - TransactionStatus.cs
      - TransactionType.cs
      - TransferType.cs
    - **Interfaces**
      - IAccountSavingRepository.cs
      - IBaseEntity.cs
      - IRepository.cs
      - ITransactionRepository.cs
    - **Models**
      - BaseEntity.cs
  - **BankTechAccountSavings.Infraestructure**
    - **Context**
      - AccountSavingDbContext.cs
      - BaseDbContext.cs
      - EntityFrameworkModelBuilderExtensions.cs
      - IDbContext.cs
    - **Migrations**
    - **Repositories**
      - **AccountSaving**
        - AccountSavingRepository.cs
      - **Transactions**
        - TransactionRepository.cs
    - IoC.cs

## API Documentation

The BankTechAccountSavings API provides the following endpoints:

### Account Savings

- **GET** `/api/AccountSaving`: retrieves a list of all Account Savings.
- **POST** `/api/AccountSaving`: Create a new account saving.
- **GET** `/api/AccountSaving/paginated`:  retrieves a paginated list of Account Savings.
- **GET** `/api/AccountSaving/{accountId}/paginated/transactions`:  retrieves a paginated list of Account Savings Transactions.
- **GET** `/api/AccountSavings/{accountId}`: retrieves details of a specific Account Saving by providing ID.
- **PUT** `/api/AccountSavings/{id}`: Update an existing Account Saving by providing ID.
- **DELETE** `/api/AccountSavings/{id}`: Delete an Account Saving by providing ID.
- **GET** `/api/AccountSavings/{accountId}`: retrieves details of a specific Account Saving by providing the Account Number.
- **GET** `/api/AccountSaving/{accountId}/transactions`: retrieves a list of Account Savings Transactions.
- **POST** `/api/AccountSaving/{accountId}/deposit`: Create a deposit to a Account Saving.
- **POST** `/api/AccountSaving/{accountId}/withdraw`: Create a withdraw to a Account Saving.
- **POST** `/api/AccountSaving/{fromAccountId}/transfer/{toAccountId}`: Create a Transfer Transaction to a account saving.
- **PATCH** `/api/AccountSaving/{fromAccountId}/transfer/{toAccountId}`: Update Account Status to be closed.

### Transactions

- **GET** `/api/Transaction`: Get a list of all Transactions.
- **GET** `/api/Transaction/paginated/transactions`:  retrieves a paginated list of all Transactions.
- **GET** `/api/Transaction/transfer`: retrieves a list of all Transfer Transactions.
- **GET** `/api/Transaction/paginated/transfers`:  retrieves a paginated list of all Transfer transactions.
- **GET** `/api/Transaction/{accountId}/paginated/transfers`:  retrieves a paginated list of all Transfer transactions of an Account.
- **GET** `/api/Transaction/transfer/{transactionId}`: retrieves details of a specific Transfer Transaction by providing the Id.
- **GET** `/api/Transaction/transfer/account/{accountId}`: retrieves details of a specific Transfer Transaction by providing the Account Saving Id.
- **GET** `/api/Transaction/deposit`: retrieves a list of all Deposit Transactions.
- **GET** `/api/Transaction/paginated/deposits`:  retrieves a paginated list of all Deposit Transactions.
- **GET** `/api/Transaction/{accountId}/paginated/deposits`:  retrieves a paginated list of all Deposit transactions of an Account.
- **GET** `/api/Transaction/deposit/{transactionId}`: retrieves details of a specific Deposit Transaction by providing the Id.
- **GET** `/api/Transaction/deposit/account/{accountId}`: retrieves details of a specific Deposit Transaction by providing the Account Saving Id.
- **GET** `/api/Transaction/withdraw`: retrieves a list of all Withdraw Transactions.
- **GET** `/api/Transaction/paginated/withdraws`:  retrieves a paginated list of all Withdraw Transactions.
- **GET** `/api/Transaction/{accountId}/paginated/withdraws`:  retrieves a paginated list of all Withdraw transactions of an Account.
- **GET** `/api/Transaction/withdraw/{transactionId}`: retrieves details of a specific Withdraw Transaction by providing the Id.
- **GET** `/api/Transaction/withdraw/account/{accountId}`: retrieves details of a specific Withdraw Transaction by providing the Account Saving Id.

## Sensitive Information

This project uses **User Secrets** to securely store sensitive information such as the Connection String for the database. User Secrets provide a way to keep configuration information out of source code and easily manage sensitive data.

### Setting Up User Secrets

1. Open a terminal and navigate to the project directory.
2. Run the following command to set up User Secrets:

   ```bash
   dotnet user-secrets init

3. Run the following command to set the Connection String:

   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your_connection_string_here"
