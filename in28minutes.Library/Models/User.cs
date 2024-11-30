using Library.Enum;

namespace Library.Models;

public class User
{
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ProfilePicture { get; set; }
    public bool? IsBanned { get; set; } = false;
    public bool? IsLocked { get; set; } = false;
    public UserRole UserRole { get; set; }
    public DateTime DateJoined { get; set; }
    public DateTime? CratedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
}
