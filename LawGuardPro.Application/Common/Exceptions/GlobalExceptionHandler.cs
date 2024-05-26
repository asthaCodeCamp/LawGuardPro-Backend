using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

namespace LawGuardPro.Application.Common.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var response = httpContext.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            Message = "An unexpected error occurred. Please try again later.",
            Detail = exception.Message // Consider customizing this based on the environment (e.g., show detailed errors in development only)
        };

        switch (exception)
        {
            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = "Resource not found.";
                break;
            case UnauthorizedAccessException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Message = "Unauthorized access.";
                break;
            case ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = "Validation error.";
                errorResponse.Errors = validationException.Errors;
                break;
            case BadRequestException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = "Bad request.";
                break;
            case InternalServerErrorException:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "Internal server error.";
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(result, cancellationToken);

        // Log the exception
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        return true;
    }
}

public class ErrorResponse
{
    public string Message { get; set; }
    public string Detail { get; set; }
    public IDictionary<string, string[]> Errors { get; set; }
}

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; set; }

    public ValidationException(string message, IDictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}

public class InternalServerErrorException : Exception
{
    public InternalServerErrorException(string message) : base(message) { }
}