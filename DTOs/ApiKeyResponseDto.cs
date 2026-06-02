using System.ComponentModel.DataAnnotations;

namespace MyOwnApi.DTOs;

public class ApiKeyResponseDto
{
    [Required]
    public string ApiKey { get; set; } = string.Empty;
    [Required]
    public DateTime ExpiresAt { get; set; }
}
