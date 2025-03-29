using FluentValidation;
using URLshortner.Dtos.Implementations;
using URLshortner.Utils;

namespace URLshortner.Dtos.Validators;

public class AccountActivationRequestValidator : AbstractValidator<ActivationRequest>
{
    public AccountActivationRequestValidator()
    {
        RuleFor(x => x.email).SetValidator(new EmailValidator());
        RuleFor(x => x.userId).SetValidator(new IdValidator());
    }
}
