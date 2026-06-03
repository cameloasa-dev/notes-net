using System.ComponentModel.DataAnnotations;

namespace MyOwnApi.DTOs;

public class ApiKeyResponseDto
{
    public string ApiKey { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
