namespace URLshortner.Dtos;

public class ResetPasswordRequest
{
    public required string email { get; set; }
    public required string token { get; set; }
    public required string newPassword { get; set; }
}
