using FluentValidation;
using URLshortner.Dtos
;
using URLshortner.Utils;

namespace URLshortner.Dtos.Validators;

public class TokenRefreshRequestValidator : AbstractValidator<TokenRefreshRequest>
{
    public TokenRefreshRequestValidator()
    {
        RuleFor(x => x.userId).SetValidator(new IdValidator());
    }
}
