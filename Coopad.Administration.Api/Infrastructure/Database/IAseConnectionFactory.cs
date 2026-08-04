using AdoNetCore.AseClient;
namespace Coopad.Administration.Api.Infrastructure.Database
{
    public interface IAseConnectionFactory
    {
        AseConnection CreateConnection();
    }
}
