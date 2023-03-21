using LegalProvisionsLib.Exceptions;

namespace LegalProvisionsBackend.Middleware;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
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

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        if (ex is ClientSideException clientSideException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(clientSideException.Message);
        }
    }
}