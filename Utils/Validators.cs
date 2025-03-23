using FluentValidation;
using System.Text.RegularExpressions;

namespace URLshortner.Utils;

public class IdValidator : AbstractValidator<int>
{
    public IdValidator()
    {
        RuleFor(x => x)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");
    }
}

public class UsernameValidator : AbstractValidator<string>
{
    public UsernameValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .Matches("^[a-zA-Z0-9]*$").WithMessage("Username must be alphanumeric.");
    }
}

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
    }
}

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");
    }
}