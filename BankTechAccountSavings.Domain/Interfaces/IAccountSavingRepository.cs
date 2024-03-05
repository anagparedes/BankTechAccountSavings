using BankTechAccountSavings.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Domain.Interfaces
{
    public interface IAccountSavingRepository: IRepository<AccountSavings>
    {
        int GetNextAccountId();
    }
}
