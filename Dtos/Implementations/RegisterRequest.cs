using System.ComponentModel.DataAnnotations;
using URLshortner.Enums;

namespace URLshortner.Dtos.Implementations;

public class RegisterRequest
{
    public required string username { get; set; }

    public required string password { get; set; }
    public required string email { get; set; }
}