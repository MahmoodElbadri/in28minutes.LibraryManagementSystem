using System;
using Library.Models;
using Library.Repository;

/// <summary>
/// Summary description for Class1
/// </summary>
public interface IBookRepository:IRepository<Book>
{
    void UpdateAsync(Book book);
}
