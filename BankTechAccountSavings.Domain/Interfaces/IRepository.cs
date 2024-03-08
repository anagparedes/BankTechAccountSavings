using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankTechAccountSavings.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetbyIdAsync(Guid id);
        Task<T> CreateAsync(T entity);
        Task<T?> UpdateAsync(Guid id, T entity);
        Task<T?> DeleteAsync(Guid id);

    }
}
