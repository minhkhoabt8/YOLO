using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using SharedLib.Core.Exceptions;
using SharedLib.ResponseWrapper;

namespace SharedLib.Middlewares;

public class ExceptionMiddleware
{
    private readonly ILogger _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        int statusCode;
        object response;

        switch (exception)
        {
            case InputValidationException valEx:
                statusCode = 400;
                response = new ApiBadRequestResponse(valEx.ValidationResults, exception.Message);
                break;
            // Expected exceptions in VOL.Core/Exceptions
            case HandledException handledEx:
                statusCode = handledEx.StatusCode;
                response = new ApiResponse(handledEx.StatusCode, isError: true, message: exception.Message);
                break;
            // Unexpected exceptions
            default:
                _logger.Error(exception, exception.Message);

                statusCode = StatusCodes.Status500InternalServerError;
                response = new ApiInternalServerErrorResponse(exception.ToString());
                //response = new ApiInternalServerErrorResponse("Internal server error");
                break;
        }

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}