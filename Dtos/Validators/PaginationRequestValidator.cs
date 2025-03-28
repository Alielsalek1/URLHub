﻿using FluentValidation;
using URLshortner.Dtos.Implementations;
using URLshortner.Utils;

namespace URLshortner.Dtos.Validators;

public class PaginationRequestValidator : AbstractValidator<PaginationRequest>
{
    public PaginationRequestValidator()
    {
        RuleFor(x => x.pageNumber).SetValidator(new IdValidator());
        RuleFor(x => x.pageSize).SetValidator(new IdValidator());
    }
}
