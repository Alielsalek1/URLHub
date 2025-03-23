using System.ComponentModel.DataAnnotations;

namespace URLshortner.Dtos.Implementations;

public class UrlRequest
{
    public required string url { get; set; }
}