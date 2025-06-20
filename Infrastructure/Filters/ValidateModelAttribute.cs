using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace URLshortner.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Any())
                .SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage))
                .ToList();

            context.Result = new BadRequestObjectResult(new
            {
                Message = "Validation errors occurred.",
                Errors = errors
            });
        }
    }
}
