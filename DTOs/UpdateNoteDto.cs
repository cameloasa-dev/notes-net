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

    [Required]
    public bool IsDone { get; set; } = false;
}
