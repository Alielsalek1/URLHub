using FluentValidation;
using URLshortner.Dtos;
using URLshortner.Utils;

namespace URLshortner.Dtos.Validators;

public class AccountActivationRequestValidator : AbstractValidator<ActionRequest>
{
    public AccountActivationRequestValidator()
    {
        RuleFor(x => x.email).SetValidator(new EmailValidator());
    }
}
