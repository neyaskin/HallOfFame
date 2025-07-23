using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HallOfFameAPI.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception.Message, "Unhandled exception");

        var problem = new ProblemDetails();
        switch (exception)
        {
            case NotFoundException:
                problem.Status = (int)HttpStatusCode.NotFound;
                problem.Detail = exception.Message;
                problem.Title = HttpStatusCode.NotFound.ToString();
                break;
            case ValidationException:
                problem.Status = (int)HttpStatusCode.BadRequest;
                problem.Detail = exception.Message;
                problem.Title = HttpStatusCode.BadRequest.ToString();
                break;
        }


        httpContext.Response.StatusCode = problem!.Status!.Value;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}