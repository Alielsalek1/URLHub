using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using URLshortner.Enums;

namespace URLshortner.Models;

public class User
{
    public int Id { get; set; } = 0;
    public string Username { get; set; }
    public string? Password { get; set; }
    public string Email { get; set; }
    public bool IsEmailVerified { get; set; } = false;
    public Roles Role { get; set; } = Roles.User;
    public string? PhoneNumber { get; set; }
    public AuthScheme AuthScheme { get; set; } = AuthScheme.UrlHub;
    // Many-to-many relationship: a user can have many MappedUrls.
    public List<MappedUrl> MappedUrls { get; set; } = new();

    // Self-referencing many-to-many: Friends relationship
    public List<UserFriend> Friends { get; set; } = new();

    // One-to-one: RefreshToken (with shared PK)
    public RefreshToken RefreshToken { get; set; } = null!;

    // one-to-many: ActivationTokens
    public List<ActivationToken> ActivationTokens { get; set; } = new();
}