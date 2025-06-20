using System.ComponentModel.DataAnnotations;

namespace URLshortner.Dtos;

public class TokenRefreshRequest
{
    public required string token { get; set; }

    public required int userId { get; set; }
}