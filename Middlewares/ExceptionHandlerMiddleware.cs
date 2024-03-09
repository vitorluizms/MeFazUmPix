using System.Net;
using MyWallet.Exceptions;

namespace MyWallet.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;


    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            HandleException(context, ex);
        }
    }

    private static void HandleException(HttpContext context, Exception exception)
    {
        ExceptionResponse response = exception switch
        {
            NotFoundError notFoundError => new ExceptionResponse(HttpStatusCode.NotFound, notFoundError.Message),
            _ => new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal Server Error")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;
        context.Response.WriteAsJsonAsync(response);
    }
}

public record ExceptionResponse(HttpStatusCode StatusCode, string Message);