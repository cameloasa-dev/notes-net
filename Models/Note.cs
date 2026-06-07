namespace MyOwnApi.Models;
public class Note
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDone { get; set; } = false;

    // Foreign key to User
    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }
}