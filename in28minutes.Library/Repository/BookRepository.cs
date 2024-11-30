using System.Linq.Expressions;
using Library.Data;
using Library.Models;
using Library.Repository;

namespace in28minutes.Library.Repository;

public class BookRepository : Repository<Book>, IBookRepository
{
    private readonly ApplicationDbContext _db;
    public BookRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Book book)
    {
        _db.Update(book);
    }
}
