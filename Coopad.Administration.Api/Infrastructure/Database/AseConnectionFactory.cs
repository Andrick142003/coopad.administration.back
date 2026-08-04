using Coopad.Administration.Api.Configuration;
using Microsoft.Extensions.Options;

namespace Coopad.Administration.Api.Infrastructure.Database
{
    public class AseConnectionFactory : IAseConnectionFactory
    {
        private readonly AseConnectionFactory _settings;

        public AseConnectionFactory(
        IOptions<AseConnectionSettings> options)
        {
            _settings = options.Value;
        }

    }
}
