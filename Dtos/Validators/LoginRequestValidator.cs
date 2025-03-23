using FluentValidation;
using URLshortner.Dtos.Implementations;
using URLshortner.Utils;

namespace URLshortner.Dtos.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.username).SetValidator(new UsernameValidator());
        RuleFor(x => x.password).SetValidator(new PasswordValidator());
    }
}