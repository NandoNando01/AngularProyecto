using System.Net;
using System.Text.Json;

namespace Api.Utils
{
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Excepción no controlada: {Message}", ex.Message);
        await HandleExceptionAsync(context, ex);
      }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      var statusCode = exception switch
      {
        NotFoundException => HttpStatusCode.NotFound,
        UnauthorizedException => HttpStatusCode.Unauthorized,
        BusinessException => HttpStatusCode.BadRequest,
        _ => HttpStatusCode.InternalServerError
      };

      context.Response.StatusCode = (int)statusCode;
      context.Response.ContentType = "application/json";

      var response = ApiResponse<object>.Fail(exception.Message);
      var payload = JsonSerializer.Serialize(response);
      await context.Response.WriteAsync(payload);
    }
  }

  public static class ExceptionMiddlewareExtensions
  {
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
      => app.UseMiddleware<ExceptionMiddleware>();
  }
}
