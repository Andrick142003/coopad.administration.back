using Coopad.Administration.Api.Models;

namespace Coopad.Administration.Api.Repositories.Interfaces
{
    public interface IHealthRepository
    {
        Health? GetHealth();

    }
}
