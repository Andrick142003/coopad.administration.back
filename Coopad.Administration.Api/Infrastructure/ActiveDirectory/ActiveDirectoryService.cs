using Coopad.Administration.Api.Configuration;
using Microsoft.Extensions.Options;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Coopad.Administration.Api.Infrastructure.ActiveDirectory
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly ActiveDirectorySettings _settings;

        public ActiveDirectoryService(
            IOptions<ActiveDirectorySettings> options)
        {
            _settings = options.Value;
        }

        public Task<bool> ValidateCredentialsAsync(
            string username,
            string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException(
                    "El username es obligatorio.",
                    nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(
                    "La contraseña es obligatoria.",
                    nameof(password));

            var userPrincipalName =
                $"{username}@{_settings.Domain}";

            var identifier = new LdapDirectoryIdentifier(
                _settings.Server,
                _settings.Port,
                fullyQualifiedDnsHostName: true,
                connectionless: false);

            using var connection =
                new LdapConnection(identifier);

            connection.SessionOptions.ProtocolVersion = 3;
            connection.SessionOptions.SecureSocketLayer =
                _settings.UseSsl;

            connection.AuthType = AuthType.Basic;

            var credential = new NetworkCredential(
                userPrincipalName,
                password);

            connection.Credential = credential;

            try
            {
                connection.Bind();

                return Task.FromResult(true);
            }
            catch (LdapException)
            {
                return Task.FromResult(false);
            }
        }
    }
}
