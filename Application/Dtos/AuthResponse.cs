namespace URLshortner.Dtos;

public class AuthResponse
{
    public required int UserId { get; set; }
    public required string RefreshToken { get; set; }
    public required string AccessToken { get; set; }
}
