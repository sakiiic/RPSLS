using System.Net;
using System.Text.Json;


public class GlobalErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;

    public GlobalErrorHandlerMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); // Proceed to the next middleware
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex); // Catch and handle exception
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var code = HttpStatusCode.InternalServerError; // Default to 500
        var result = JsonSerializer.Serialize(new
        {
            error = ex.Message,
        });

        // Log the exception
        _logger.LogError(ex, "An unhandled exception occurred.");

        // Customize the response based on the exception type
        switch (ex)
        {
            case ArgumentException:
                code = HttpStatusCode.BadRequest; // 400
                break;
            case UnauthorizedAccessException:
                code = HttpStatusCode.Unauthorized; // 401
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}