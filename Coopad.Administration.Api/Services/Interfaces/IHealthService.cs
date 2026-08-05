using Coopad.Administration.Api.DTOs.Common;
using Coopad.Administration.Api.Models;

namespace Coopad.Administration.Api.Services.Interfaces
{
    public interface IHealthService
    {
        Health GetStatus();
    }
}
