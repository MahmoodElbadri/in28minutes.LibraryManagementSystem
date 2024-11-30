using System.Collections.Generic;
using System.Linq.Expressions;
using Library.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        this.dbSet = _db.Set<T>();
    }

    // Add an entity asynchronously
    public async Task AddAsync(T entity)
    {
        await dbSet.AddAsync(entity);
    }

    // Get a single entity asynchronously based on a filter
    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        query = query.Where(filter);

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.FirstOrDefaultAsync();
    }

    // Get all entities asynchronously
    public async Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.ToListAsync();
    }

    // Remove an entity asynchronously
    public async Task RemoveAsync(T entity)
    {
        dbSet.Remove(entity);
        await _db.SaveChangesAsync(); // Ensure changes are saved asynchronously
    }

    // Remove a range of entities asynchronously
    public async Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
        await _db.SaveChangesAsync(); // Ensure changes are saved asynchronously
    }
}
