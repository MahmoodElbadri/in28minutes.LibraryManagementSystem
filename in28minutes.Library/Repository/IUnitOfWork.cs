using Library.Models;

namespace in28minutes.Library.Repository;

public interface IUnitOfWork
{
    IBookRepository BookRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IUserService<User> UserService { get; }
    void Save();
}
