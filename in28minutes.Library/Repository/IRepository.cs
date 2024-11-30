using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Repository;

public interface IRepository<T> where T : class
{
    // Asynchronously gets all entities
    Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null);

    // Asynchronously gets a single entity based on a filter
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);

    // Asynchronously adds an entity
    Task AddAsync(T entity);

    // Asynchronously removes an entity
    Task RemoveAsync(T entity);

    // Asynchronously removes a range of entities
    Task RemoveRangeAsync(IEnumerable<T> entities);
}
