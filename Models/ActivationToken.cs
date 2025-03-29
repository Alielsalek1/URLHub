namespace URLshortner.Models;

public class ActivationToken
{
    public required Guid Token { get; set; }
    public required DateTime Expires { get; set; }

    public required int UserId { get; set; }
    public required User User = null!;
}