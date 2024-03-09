
using BankTechAccountSavings.Domain.Entities;

namespace BankTechAccountSavings.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T?> GetbyIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> UpdateAsync(Guid id, T entity, CancellationToken cancellationToken = default);
        Task<T?> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Paginated<T>> GetPaginatedAccountsAsync(
        IQueryable<T> queryable,
        int page,
        int pageSize);

    }
}
