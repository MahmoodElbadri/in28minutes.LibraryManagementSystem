using in28minutes.Library.Services;
using Library.Data;
using Library.Models;

namespace in28minutes.Library.Repository;

public class UnitOfWork : IUnitOfWork
{
    public IBookRepository BookRepository { get; private set; }

    public ICategoryRepository CategoryRepository { get; private set; }

    public IUserService<User> UserService { get; private set; }

    private readonly ApplicationDbContext _db;
    public UnitOfWork(ApplicationDbContext db, ILogger<UserService> logger)
    {
        _db = db;
        CategoryRepository = new CategoryRepository(_db);
        BookRepository = new BookRepository(_db);
        UserService = new UserService(_db, logger);
    }


    public void Save()
    {
        _db.SaveChanges();
    }
}
