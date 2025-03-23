using FluentValidation;
using URLshortner.Dtos.Implementations;

namespace URLshortner.Dtos.Validators;

public class UrlRequestValidator : AbstractValidator<UrlRequest>
{
    public UrlRequestValidator()
    {
        RuleFor(x => x.url).NotEmpty();
    }
}
