using System.Text.RegularExpressions;
using URLshortner.Models;

namespace URLshortner.Validators;

public class UserValidator
{
    private bool IsValidUsername(string? Username)
    {
        string pattern = @"^[^\d].*";
        return Username != null && Regex.IsMatch(Username, pattern);
    }
    private bool IsValidPassword(string? Password)
    {
        string pattern = @"[!@#$%^&*(){}]";
        return Password != null && Regex.IsMatch(Password, pattern) && Password.Length >= 8;
    }
    private bool IsValidEmail(string? Email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Email != null && Regex.IsMatch(Email, pattern);
    }
    private bool IsValidRole(int? Role)
    {
        return Role != null && (Role == 0 || Role == 1);
    }
    private bool IsValidSex(int? Sex)
    {
        return Sex != null && (Sex >= 0 && Sex <= 2);
    }
    private bool IsValidDateOfBirth(DateTime? DateString)
    {
        return DateString != null;
    }
    public bool IsValidUser(User user)
    {
        return user != null &&
            user.ID >= 0 && 
            IsValidUsername(user.Username) &&
            IsValidPassword(user.Password) &&
            IsValidEmail(user.Email) &&
            IsValidRole((int)user.Role) &&
            IsValidSex((int)user.Sex) &&
            IsValidDateOfBirth(user.DateOfBirth);
    }
}
