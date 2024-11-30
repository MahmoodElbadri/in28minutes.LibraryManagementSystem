using System.ComponentModel.DataAnnotations;

namespace in28minutes.Library.DTOs;

public class LoginRequestDto
{
    [StringLength(80, MinimumLength = 3)]
    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(255, MinimumLength = 8)]
    [Required]
    public string? Password { get; set; }
}
