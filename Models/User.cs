using Microsoft.AspNetCore.Identity;

namespace MyOwnApi.Models;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // 1:Many relationship with Notes
    public ICollection<Note> Notes { get; set; } = [];

    // 1:1 relationship with ApiKey
    public ApiKey? APIKey { get; set; }
}