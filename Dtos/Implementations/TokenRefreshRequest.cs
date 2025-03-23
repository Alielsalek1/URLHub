using System.ComponentModel.DataAnnotations;

namespace URLshortner.Dtos.Implementations;

public class TokenRefreshRequest
{
    public required string token { get; set; }

    public required int userId { get; set; }
}