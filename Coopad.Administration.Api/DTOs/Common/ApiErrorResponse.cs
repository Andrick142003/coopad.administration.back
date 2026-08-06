namespace Coopad.Administration.Api.DTOs.Common
{
    public class ApiErrorResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public string TraceId { get; set; } = string.Empty;

        public List<string> Errors { get; set; } = [];
    }
}
