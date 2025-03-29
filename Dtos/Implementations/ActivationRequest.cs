namespace URLshortner.Dtos.Implementations;

public class ActivationRequest
{
    public required string email { get; set; }
    public required int userId { get; set; }
}
