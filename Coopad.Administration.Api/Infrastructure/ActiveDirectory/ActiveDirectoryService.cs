using Coopad.Administration.Api.Configuration;
using Microsoft.Extensions.Options;

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
            throw new NotImplementedException();
        }
    }
}
