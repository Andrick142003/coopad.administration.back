using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;

namespace Coopad.Administration.Api.Authorization

{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }
}
