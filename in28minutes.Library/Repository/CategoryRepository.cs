using Library.Data;
using Library.Models;
using Library.Repository;

namespace in28minutes.Library.Repository;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly ApplicationDbContext _db;
    public CategoryRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        _db.Update(category);
        await _db.SaveChangesAsync();
        return category;
    }
}
