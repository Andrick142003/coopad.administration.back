namespace Coopad.Administration.Api.DTOs.Common
{
    public class ApiResponse<T>
    {
        public bool Succes {  get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
