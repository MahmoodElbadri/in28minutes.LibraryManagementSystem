namespace Library.DTOs;

public class BookResponse
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Author { get; set; }

    public string? ISBN { get; set; }

    public string? Publisher { get; set; }

    public DateTime? PublishDate { get; set; }

    public string? Description { get; set; }
    public bool? CopiesAvailable { get; set; }
    public int? TotalCopies { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime? CratedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
}
