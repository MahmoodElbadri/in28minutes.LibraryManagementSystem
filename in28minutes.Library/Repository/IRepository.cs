using System.Linq.Expressions;

namespace Library.Repository;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll(string? includeProperties = null);
    T? Get(Expression<Func<T, bool>> filter,string? includeProperties = null); //The expression is for the linq that will be written for FirstOrDefault
    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}
