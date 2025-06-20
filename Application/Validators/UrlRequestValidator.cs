using FluentValidation;
using URLshortner.Dtos
;

namespace URLshortner.Dtos.Validators;

public class UrlRequestValidator : AbstractValidator<UrlRequest>
{
    public UrlRequestValidator()
    {
        RuleFor(x => x.url).NotEmpty();
    }
}
