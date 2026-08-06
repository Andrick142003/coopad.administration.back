namespace Coopad.Administration.Api.DTOs.Responses
{
    public class HealthResponse
    {
        public string Message { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }

        public string Version { get; set; } = string.Empty;
    }
}
