using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using LawGuardPro.Application.Common;
using Microsoft.AspNetCore.Diagnostics;
using LawGuardPro.Domain.Exceptions;

namespace LawGuardPro.API.Middlewares.Exceptions;
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

        Result result;

        switch (exception)
        {
            case NotFoundException notFoundException:
                result = Result.Failure(
                    (int)HttpStatusCode.NotFound,
                    new List<Error> { new Error { Code = "exception.notfound", Message = notFoundException.Message } }
                );
                response.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            case UnauthorizedAccessException unauthorizedAccessException:
                result = Result.Failure(
                    (int)HttpStatusCode.Unauthorized,
                    new List<Error> { new Error { Code = "unauthorized.access", Message = unauthorizedAccessException.Message } }
                );
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;

            case ValidationException validationException:
                result = Result.Failure(
                    (int)HttpStatusCode.BadRequest,
                    new List<Error> { new Error { Code = "validation.error", Message = validationException.Message } }
                );
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case BadRequestException badRequestException:
                result = Result.Failure(
                    (int)HttpStatusCode.BadRequest,
                    new List<Error> { new Error { Code = "badrequest.error", Message = badRequestException.Message } }
                );
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;

            case InternalServerErrorException internalServerErrorException:
                _logger.LogInformation("Handling unknown exception");
                result = Result.Failure(
                    (int)HttpStatusCode.InternalServerError,
                    new List<Error> { new Error { Code = "internalserver.error", Message = internalServerErrorException.Message } }
                );
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;

            default:
                result = Result.Failure(
                    (int)HttpStatusCode.InternalServerError,
                    new List<Error> { new Error { Code = "unknown.error", Message = "An unexpected error occurred. Please try again later." } }
                );
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        // Log the exception
        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var serializedResult = JsonSerializer.Serialize(result);
        await response.WriteAsync(serializedResult, cancellationToken);

        return true;
    }
}