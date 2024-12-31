using System.ComponentModel.DataAnnotations;

namespace URLshortner.Dtos;

public class AuthRequestDto
{
    [Required(ErrorMessage = "Username is required.")]
    public string username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string password { get; set; }
}