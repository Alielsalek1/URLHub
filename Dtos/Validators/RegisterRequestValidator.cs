using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using URLshortner.Dtos.Implementations;
using URLshortner.Utils;
using RegisterRequest = URLshortner.Dtos.Implementations.RegisterRequest;

namespace URLshortner.Dtos.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.email).SetValidator(new EmailValidator());
        RuleFor(x => x.username).SetValidator(new UsernameValidator());
        RuleFor(x => x.password).SetValidator(new PasswordValidator());
    }
}
