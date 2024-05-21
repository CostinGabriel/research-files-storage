using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using ResearchFilesStorage.Infrastructure;

namespace ResearchFilesStorage.Controllers;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            var controllerActionDescriptor = httpContext?.GetEndpoint()
                                                        ?.Metadata
                                                        .GetMetadata<ControllerActionDescriptor>();

            var controllerName = controllerActionDescriptor?.ControllerName ?? string.Empty;

            // Skip counting when we are checking counter value;
            if (!nameof(HttpRequestCounterController).Contains(controllerName))
            {
                HttpRequestCounter.Instance.IncreaseCounter();
            }

            await next(httpContext);
        }
        catch (Application.Exceptions.ValidationException exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "ValidationFailure",
                Title = "Validation error",
                Detail = "One or more validation errors has occurred"
            };

            if (exception.Errors is not null)
            {
                problemDetails.Extensions["errors"] = exception.Errors;
            }

            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (Exception ex)
        {
            logger.LogError("Error while processing request: {message} \n StackTrace: {stackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }
}
