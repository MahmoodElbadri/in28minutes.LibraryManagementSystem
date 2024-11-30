namespace Library.Models;

public class Category
{
    public Guid  Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<Book>? Books { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
