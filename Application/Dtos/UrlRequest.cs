using System.ComponentModel.DataAnnotations;

namespace URLshortner.Dtos;

public class UrlRequest
{
    public required string url { get; set; }
}