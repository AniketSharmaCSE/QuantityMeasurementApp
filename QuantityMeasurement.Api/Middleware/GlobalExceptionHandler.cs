using System.Net;
using System.Text.Json;
using QuantityMeasurement.Model.Exceptions;

namespace QuantityMeasurement.Api.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next   = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (QuantityMeasurementException ex)
            {
                // known domain error – 400, no stack trace needed
                _logger.LogWarning(ex, "Quantity error on {Path}", context.Request.Path);
                await Write(context, HttpStatusCode.BadRequest, "Quantity Measurement Error", ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized on {Path}", context.Request.Path);
                await Write(context, HttpStatusCode.Unauthorized, "Unauthorized", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Invalid operation on {Path}", context.Request.Path);
                await Write(context, HttpStatusCode.BadRequest, "Bad Request", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception on {Path}", context.Request.Path);
                await Write(context, HttpStatusCode.InternalServerError, "Internal Server Error",
                    "An unexpected error occurred.");
            }
        }

        private static async Task Write(HttpContext ctx, HttpStatusCode code, string error, string message)
        {
            ctx.Response.StatusCode      = (int)code;
            ctx.Response.ContentType     = "application/json";

            var body = new
            {
                timestamp = DateTime.UtcNow,
                status    = (int)code,
                error,
                message,
                path = ctx.Request.Path.Value
            };

            await ctx.Response.WriteAsync(
                JsonSerializer.Serialize(body,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
    }
}
