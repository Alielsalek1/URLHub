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
            FailedToSendEmailException => new ProblemDetails
            {
                Title = "Failed to send Email",
                Detail = exception.Message,
                Status = StatusCodes.Status500InternalServerError
            },
            ExistsButNotVerifiedException => new ProblemDetails
            {
                Title = "Email already Exists but requires verification",
                Detail = exception.Message,
                Status = StatusCodes.Status409Conflict
            },
            AlreadyVerifiedException => new ProblemDetails
            {
                Title = "Email already Exists but requires verification",
                Detail = exception.Message,
                Status = StatusCodes.Status403Forbidden
            },
            GoogleAuthFailedException => new ProblemDetails
            {
                Title = "Google Authentication Failed",
                Detail = exception.Message,
                Status = StatusCodes.Status401Unauthorized
            },
            _ => new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "an unexpected error has happened",
                Status = StatusCodes.Status500InternalServerError
            }
        };

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
