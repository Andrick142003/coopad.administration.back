using Coopad.Administration.Api.DTOs.Common;
using System.Text.Json;

namespace Coopad.Administration.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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

                _logger.LogError(
                     ex,
                     "Error no controlado. TraceId: {TraceId}",
                      context.TraceIdentifier
                );

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                context.Response.ContentType = "application/json";

                var response = new ApiErrorResponse
                {
                    Success = false,
                    Message = "Ha ocurrido un error interno.",
                    TraceId = context.TraceIdentifier,
                    Errors = []
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response)
                );
            }

        }
    }
}
