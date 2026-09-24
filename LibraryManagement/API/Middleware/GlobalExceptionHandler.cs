using LibraryManagement.API.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        int status;
        string title;

        if (exception is NotFoundException)
        {
            status = StatusCodes.Status404NotFound;
            title = "Resource not found";
        }
        else if (exception is DomainException)
        {
            status = StatusCodes.Status422UnprocessableEntity;
            title = "Business rule violation";
        }
        else if (exception is DbUpdateConcurrencyException)
        {
            status = StatusCodes.Status409Conflict;
            title = "The record was changed by another request. Please retry.";
        }
        else
        {
            status = StatusCodes.Status500InternalServerError;
            title = "An unexpected error occurred";
        }

        if (status == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled	exception");
        }

        var problem = new ProblemDetails();

        problem.Status = status;
        problem.Title = title;

        if (status == StatusCodes.Status500InternalServerError)
        {
            problem.Detail = null;
        }
        else
        {
            problem.Detail = exception.Message;
        }

        problem.Instance = httpContext.Request.Path;
        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problem, ct);
        return true;
    }
}