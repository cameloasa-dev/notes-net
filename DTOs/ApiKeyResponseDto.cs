
namespace MyOwnApi.DTOs;

public class ApiKeyResponseDto
{
    public string ApiKey { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string UserId { get; set; } = string.Empty;
}
