namespace MyOwnApi.DTOs;

public class NoteResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool IsDone { get; set; }

    public DateTime? DueDate { get; set; }
    public string Priority { get; set; } = default!;
    public bool IsPinned { get; set; }

    public bool IsOverdue =>
        DueDate.HasValue &&
        DueDate.Value < DateTime.UtcNow &&
        !IsDone;


    public string UserId { get; set; } = string.Empty;
}
