using System;
using Library.Models;
using Library.Repository;

/// <summary>
/// Summary description for Class1
/// </summary>
public interface ICategoryRepository:IRepository<Category>
{
	void Update(Category category);
}
