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

    public async Task<Book> UpdateAsync(Book book)
    {
        _db.Books.Update(book);  // Updates the book entity
        await _db.SaveChangesAsync();  // Save the changes to the database asynchronously
        return book;
    }
}
