using Coopad.Administration.Api.Models;

namespace Coopad.Administration.Api.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
