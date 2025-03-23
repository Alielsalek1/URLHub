using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using URLshortner.Exceptions;

namespace URLshortner.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
        )
    {
        var problemDetails = exception switch
        {
            NotFoundException => new ProblemDetails
            {
                Title = "Resource Not Found",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            },
            InvalidInputException => new ProblemDetails
            {
                Title = "Invalid Credentials",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            },
            UnAuthorizedException => new ProblemDetails
            {
                Title = "Unauthorized Access",
                Detail = exception.Message,
                Status = StatusCodes.Status401Unauthorized
            },
            AlreadyExistsException => new ProblemDetails
            {
                Title = "Conflict",
                Detail = exception.Message,
                Status = StatusCodes.Status409Conflict
            },
            UriFormatException => new ProblemDetails
            {
                Title = "Invalid Url",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            },
            _ => new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = exception.Message,
                Status = StatusCodes.Status500InternalServerError
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
