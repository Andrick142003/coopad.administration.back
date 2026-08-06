using Coopad.Administration.Api.DTOs.Common;
using Coopad.Administration.Api.DTOs.Responses;
using System.Text.Json;

namespace Coopad.Administration.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                context.Response.ContentType = "application/json";

                var response = new ApiErrorResponse
                {
                    Success = false,
                    Message = "Ha ocurrido un error interno.",
                    TraceId = context.TraceIdentifier
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response)
                );
            }

        }
    }
}
