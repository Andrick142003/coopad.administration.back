using AdoNetCore.AseClient;
using Coopad.Administration.Api.Configuration;
using Microsoft.Extensions.Options;
using System.Text;

namespace Coopad.Administration.Api.Infrastructure.Database
{
    public class AseConnectionFactory : IAseConnectionFactory
    {
        private readonly AseConnectionSettings _settings;

        public AseConnectionFactory(
        IOptions<AseConnectionSettings> options)
        {
            _settings = options.Value;
        }

        public AseConnection CreateConnection()
        {
            return new AseConnection(BuildConnectionString());
        }



        private string BuildConnectionString()
        {
            var builder = new StringBuilder();

            builder.Append($"Data Source={_settings.Server};");
            builder.Append($"Port={_settings.Port};");
            builder.Append($"Database={_settings.Database};");
            builder.Append($"User ID={_settings.Username};");
            builder.Append($"Password={_settings.Password};");
            builder.Append($"Connection Timeout={_settings.ConnectionTimeout};");

            if (!string.IsNullOrWhiteSpace(_settings.Charset))
            {
                builder.Append($"Charset={_settings.Charset};");
            }

            return builder.ToString();
        }

    }
}
