using Library.API.Services.Exceptions;
namespace Library.API.Middlewares;

public class LibraryExceptionMiddleware
{
  private static readonly ILogger logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("LibraryExceptionMiddleware");
  
  private readonly RequestDelegate _next;

  public LibraryExceptionMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext context)
  {
    try
    {
      await _next.Invoke(context);
    }
    catch(Exception e)
    {
      logger.LogError(e, $"error in processing {context.Request.Path.Value}");
      var response = context.Response;
      response.ContentType = "text/html";
      response.StatusCode = GetStatusCode(e);
      await response.WriteAsync(e.Message);
    }
  }

  private int GetStatusCode(Exception e)
  {
    int code;
    switch(e)
    {
      case LibraryServiceException:
        code = StatusCodes.Status500InternalServerError;
        break;
      default:
        code = StatusCodes.Status400BadRequest;
        break;
    }
    return code;
  }
}
