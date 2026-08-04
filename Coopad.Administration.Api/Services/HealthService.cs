using Coopad.Administration.Api.Interfaces.Services;

namespace Coopad.Administration.Api.Services
{
    public class HealthService : IHealthService
    {
        public string GetSatus() {
            return "La api funciona correctamente";
        }
    }
}
