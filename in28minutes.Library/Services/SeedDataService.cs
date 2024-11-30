using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace in28minutes.Library.Services;

public class SeedDataService : ISeedDataService
{
    public void Initialize(ApplicationDbContext context)
    {
        var categoryGuid1 = Guid.NewGuid();
        var categoryGuid2 = Guid.NewGuid();
        List<Category> categories = new List<Category>() {
            new Category
            {
                Id = categoryGuid1,
                Name = "Science Fiction"
            },
            new Category
            {
                Id = categoryGuid2,
                Name = "Fantasy"
            }};
        context.AddRange(categories);
        context.SaveChanges();

        // Seed Books
        List<Book> books = new List<Book>() {
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "Dune",
                Author = "Frank Herbert",
                ISBN = "9780441013593",
                Publisher = "Ace",
                PublishDate = new DateTime(1965, 8, 1),
                Description = "A science fiction novel.",
                CopiesAvailable = true,
                TotalCopies = 5,
                CategoryId = categoryGuid1 // Use only the foreign key here
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                ISBN = "9780345339683",
                Publisher = "Houghton Mifflin",
                PublishDate = new DateTime(1937, 9, 21),
                Description = "A fantasy novel.",
                CopiesAvailable = true,
                TotalCopies = 7,
                CategoryId = categoryGuid2 // Use only the foreign key here
            } };
        context.AddRange(books);
        context.SaveChanges();
    }
}
