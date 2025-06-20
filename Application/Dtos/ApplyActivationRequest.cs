namespace URLshortner.Dtos;

public class ApplyActivationRequest
{
    public required string email { get; set; }
    public required string token { get; set; }
}
