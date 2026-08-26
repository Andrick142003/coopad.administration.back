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

            var identifier = new LdapDirectoryIdentifier(
                _settings.Server,
                _settings.Port,
                true,
                false);

            using var connection =
                new LdapConnection(identifier);

            connection.SessionOptions.ProtocolVersion = 3;

            connection.SessionOptions.SecureSocketLayer =
                _settings.UseSsl;

            connection.AuthType = AuthType.Basic;

            connection.Credential = new NetworkCredential(
                $"{username}@{_settings.Domain}",
                password);

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


        public Task<ActiveDirectoryUser?>GetUserAsync(
    string username,
    string password)
        {
            var identifier = new LdapDirectoryIdentifier(
                _settings.Server,
                _settings.Port,
                true,
                false);

            using var connection =
                new LdapConnection(identifier);

            connection.SessionOptions.ProtocolVersion = 3;

            connection.SessionOptions.SecureSocketLayer =
                _settings.UseSsl;

            connection.AuthType = AuthType.Basic;

            connection.Credential = new NetworkCredential(
                $"{username}@{_settings.Domain}",
                password);

            try
            {
                connection.Bind();


                var searchRequest = new SearchRequest(
                    "DC=coopad,DC=fin,DC=ec",
                    $"(&(objectClass=user)(sAMAccountName={username}))",
                    SearchScope.Subtree,
                    "sAMAccountName",
                    "displayName",
                    "mail");

                var response =
                    (SearchResponse)connection.SendRequest(
                        searchRequest);

                if (response.Entries.Count == 0)
                {
                    return Task.FromResult<ActiveDirectoryUser?>(
                        null);
                }

                var entry = response.Entries[0];

                var user = new ActiveDirectoryUser
                {
                    Username =
                        GetAttribute(
                            entry,
                            "sAMAccountName")
                        ?? username,

                    DisplayName =
                        GetAttribute(
                            entry,
                            "displayName"),

                    Email =
                        GetAttribute(
                            entry,
                            "mail")
                };

                return Task.FromResult<ActiveDirectoryUser?>(
                    user);
            }
            catch (LdapException)
            {
                return Task.FromResult<ActiveDirectoryUser?>(
                    null);
            }
        }

        private static string? GetAttribute(
         SearchResultEntry entry,
             string attributeName)
        {
            if (!entry.Attributes.Contains(attributeName))
                return null;

            var attribute =
                entry.Attributes[attributeName];

            if (attribute.Count == 0)
                return null;

            return attribute[0]?.ToString();
        }

    
    }
}
