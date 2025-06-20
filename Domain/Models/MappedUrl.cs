namespace URLshortner.Models;

public class MappedUrl
{
    public required string shortUrl { get; set; }
    public required string longUrl { get; set; }

    // Navigation property for many-to-many relationship with users.
    public List<User> Users { get; set; } = new();
}
