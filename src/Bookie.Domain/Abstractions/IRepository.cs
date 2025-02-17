namespace Bookie.Domain.Abstractions;

public interface IRepository<T> where T : Entity
{
    Task<T> CreateAsync(T entity);
    Task<T?> GetAsync(Guid id);
    Task<T> UpdateAsync(T entity);
    Task<T?> DeleteAsync(Guid id);
}
