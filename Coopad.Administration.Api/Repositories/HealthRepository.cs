using Coopad.Administration.Api.Repositories.Interfaces;

namespace Coopad.Administration.Api.Repositories
{
    public class HealthRepository : IHealthRepository
    {
        public bool IsDatabaseAvailable() {
            return true;
        }
    }
}
