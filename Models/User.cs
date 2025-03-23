using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using URLshortner.Enums;

namespace URLshortner.Models;

public class User
{
    public required int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public Roles Role { get; set; } = Roles.User;

    // Many-to-many relationship: a user can have many MappedUrls.
    public required List<MappedUrl> MappedUrls { get; set; } = new();

    // Self-referencing many-to-many: Friends relationship
    public required List<UserFriend> Friends { get; set; } = new();

    // One-to-one: RefreshToken (with shared PK)
    public required RefreshToken RefreshToken { get; set; } = null!;
}