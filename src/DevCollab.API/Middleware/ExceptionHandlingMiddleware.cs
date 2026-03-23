using System.Net;
using System.Text.Json;
using DevCollab.Domain.Exceptions;

namespace DevCollab.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new { message = exception.Message, details = (object?)null };

        switch (exception)
        {
            case DomainValidationException validationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new { message = "Validation failed", details = (object?)validationException.Errors };
                break;
            case NotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case DomainException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = new { message = "An internal server error occurred.", details = (object?)null };
                break;
        }

        var result = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(result);
    }
}
