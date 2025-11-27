using System.Net;
using System.Text.Json;

namespace Blogs.Api.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next)
    {    
            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await next(context);
                }
                catch (Exception exception)
                {
                    var response = GetResponseForException(context, exception);

                    var exceptionResponse = new
                    {
                        exceptionType = exception.GetType().Name,
                        statusCode = response.StatusCode,
                        message = exception.Message,
                    };

                    var result = JsonSerializer.Serialize(exceptionResponse);
                    await response.WriteAsync(result);
                }
            }

            private HttpResponse GetResponseForException(HttpContext context, Exception exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                response.StatusCode = exception switch
                {
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,

                    ArgumentException => (int)HttpStatusCode.BadRequest,

                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,

                    NotImplementedException => (int)HttpStatusCode.NotImplemented,

                    MemberAccessException => (int)HttpStatusCode.Forbidden,

                    _ => (int)HttpStatusCode.InternalServerError
                };

                return response;
        }
    }
}
