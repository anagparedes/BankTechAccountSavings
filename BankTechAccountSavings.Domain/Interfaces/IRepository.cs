
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
