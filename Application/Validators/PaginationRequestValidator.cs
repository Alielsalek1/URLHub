using FluentValidation;
using URLshortner.Dtos;
using URLshortner.Utils;

namespace URLshortner.Dtos.Validators;

public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
    public PaginationRequestValidator()
    {
        RuleFor(x => x.pageNumber).SetValidator(new PageNumberValidator());
        RuleFor(x => x.pageSize).SetValidator(new PageSizeValidator());
    }
}
