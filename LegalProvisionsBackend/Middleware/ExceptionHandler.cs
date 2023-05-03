using System.Text;
using LegalProvisionsLib.Exceptions;
using ILogger = LegalProvisionsLib.Logging.ILogger;

namespace LegalProvisionsBackend.Middleware;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionHandler(RequestDelegate next, ILogger logger)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        switch (ex)
        {
            case ClientSideException clientSideException:
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(clientSideException.Message);
                break;
            case BaseException baseException:
                context.Response.StatusCode = (int) baseException.HttpStatusCode;
                var buffer = Encoding.UTF8.GetBytes(baseException.Message);
                var memStream = new MemoryStream(buffer);
                context.Response.Body = memStream;
                break;
            default:
                throw ex;
        }
        
        _logger.LogException(ex);
    }
}