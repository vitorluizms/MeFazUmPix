using System.Net;
using MyWallet.Exceptions;

namespace MyWallet.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unexpected error occurred.");


        ExceptionResponse response = exception switch
        {
            NotFoundError notFoundError => new ExceptionResponse(HttpStatusCode.NotFound, notFoundError.Message),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal Server Error")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}

public record ExceptionResponse(HttpStatusCode StatusCode, string Message);