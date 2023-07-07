using System.Net;
using AddressBookApi.Dtos;
using Contracts;
using ExceptionHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerManager _logger;
    public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
    {
        _logger = logger;
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (CustomException ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex}");
            HttpResponse response = httpContext.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponseDto
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = ex.Message,
            }));
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, CustomException exception)
    {
        CustomException customException = exception;
        int statusCode = customException.Code;
        string message = customException.Message;
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        _logger.LogError($"Something went wrong: ");

        await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponseDto()
        {
            StatusCode = statusCode,
            Message = message,
        }));
    }
}