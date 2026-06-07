using System.ComponentModel.DataAnnotations;
namespace MyOwnApi.DTOs;

public class UpdateNoteDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Content { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }
    public string Priority { get; set; } = "Low"; // Low, Medium, High
    public bool IsPinned { get; set; } = false;
    public bool IsDone { get; set; } = false;
}
