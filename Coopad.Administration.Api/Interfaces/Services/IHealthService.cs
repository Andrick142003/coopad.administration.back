using Coopad.Administration.Api.DTOs.Common;

namespace Coopad.Administration.Api.Interfaces.Services
{
    public interface IHealthService
    {
        HealthResponse GetStatus();
    }
}
