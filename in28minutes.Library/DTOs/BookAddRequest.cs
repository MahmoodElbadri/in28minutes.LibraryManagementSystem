using System.ComponentModel.DataAnnotations;

namespace Library.DTOs;

public class BookAddRequest
{
    [StringLength(255, MinimumLength = 3)]
    [Required]
    public string? Title { get; set; }

    [StringLength(80, MinimumLength = 3)]
    [Required]
    public string? Author { get; set; }

    [StringLength(255, MinimumLength = 5)]
    [Required]
    public string? ISBN { get; set; }

    [StringLength(255, MinimumLength = 3)]
    [Required]
    public string? Publisher { get; set; }

    [StringLength(255, MinimumLength = 3)]
    [Required]
    public string? PublishDate { get; set; }

    public string? Description { get; set; }
    public bool? CopiesAvailable { get; set; }
    public int? TotalCopies { get; set; }
    public Guid CategoryId { get; set; }
}
