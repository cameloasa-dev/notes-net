namespace MyOwnApi.Models;

public class Note
{
    public int Id { get; set; }

    // Content
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Status flags
    public bool IsDone { get; set; } = false;
    public bool IsPinned { get; set; } = false;
    public bool IsDeleted { get; set; } = false; 

    // Scheduling
    public DateTime? DueDate { get; set; }

    // Priority
    public string Priority { get; set; } = "Low"; // Low, Medium, High

    // Foreign key to User
    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }
}
