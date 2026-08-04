using Coopad.Administration.Api.DTOs.Common;

namespace Coopad.Administration.Api.Services.Interfaces
{
    public interface IHealthService
    {
        HealthResponse GetStatus();
    }
}
