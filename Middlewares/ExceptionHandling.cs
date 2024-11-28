using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace AddressService.Middlewares
{
    /// <summary>
    /// middleware для обработки необработанных исключений в приложении
    /// логирует ошибки и возвращает пользователю json-ответ с информацией об ошибке
    /// </summary>
    public class ExceptionHandling
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandling> _logger;

        public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // передача следующему
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Oops, an unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        public static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            if (exception is ArgumentException)
            {
                statusCode = HttpStatusCode.BadRequest;
            }

            var response = new
            {
                Error = exception.Message,
                Details = "An error occurred while processing your request"
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
