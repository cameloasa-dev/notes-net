
namespace MyOwnApi.Models;
public class ApiKey
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  
    // Foreign key to User
    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }
}