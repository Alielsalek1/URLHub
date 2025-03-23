using System.ComponentModel.DataAnnotations;
using URLshortner.Utils;

namespace URLshortner.Dtos.Implementations;

public class UpdateUserRequest
{
    public string? username { get; set; }

    public string? password { get; set; }
}
