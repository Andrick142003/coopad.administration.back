using Microsoft.AspNetCore.Authorization;

namespace Coopad.Administration.Api.Authorization
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission)
        {
            Policy = permission;
        }
    }
}
