using FluentValidation;
using URLshortner.Dtos
;
using URLshortner.Utils;

namespace URLshortner.Dtos.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.username)
            .SetValidator(new UsernameValidator())
            .When(x => !string.IsNullOrWhiteSpace(x.username));

        RuleFor(x => x.password)
            .SetValidator(new PasswordValidator())
            .When(x => !string.IsNullOrWhiteSpace(x.password));

        RuleFor(x => x)
            .Custom((x, context) =>
            {
                if (string.IsNullOrWhiteSpace(x.username) && string.IsNullOrWhiteSpace(x.password))
                {
                    context.AddFailure("At least one of username or password must be provided.");
                }
            });
    }
}
