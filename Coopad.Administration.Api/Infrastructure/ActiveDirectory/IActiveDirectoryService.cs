namespace Coopad.Administration.Api.Infrastructure.ActiveDirectory
{
    public interface IActiveDirectoryService
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);
    }
}
