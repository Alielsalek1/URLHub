namespace URLshortner.Models;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class RefreshToken
{
    public required string Token { get; set; }
    public required DateTime Expires { get; set; }
    public bool IsRevoked { get; set; } = false;

    public int? UserId { get; set; } // Foreign key & primary key
    public required User User = null!; // navigation property
}