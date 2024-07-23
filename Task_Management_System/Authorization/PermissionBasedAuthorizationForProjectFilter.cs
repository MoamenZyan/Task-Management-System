using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Project.API.Enums;
using Project.API.Interfaces;
using System.Security.Claims;

namespace Project.API.Authorization
{
    public class PermissionBasedAuthorizationForProjectFilter(IUserProjectService userProjectService, IAuthorizationPolicy authorizationPolicy) : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {

            var permission = (PermissionAttribute)context.ActionDescriptor.EndpointMetadata.FirstOrDefault(x => x is PermissionAttribute)!;
            if (permission != null)
            {
                var claimIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (claimIdentity == null || !claimIdentity.IsAuthenticated)
                {
                    context.Result = new ForbidResult();
                }
                else
                {
                    var userId = int.Parse(claimIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                    var body = await new FormReader(context.HttpContext.Request.Body).ReadFormAsync();
                    var projectId = Convert.ToInt16(body["ProjectId"]);
                    var role = await userProjectService.GetUserRoleInProject(userId, projectId);
                    if (role == null)
                        context.Result = new ForbidResult();
                    Console.WriteLine(role);

                    if (!authorizationPolicy.CheckAuthorizationOnAction(permission.Permission, (UserRole)role!))
                        context.Result = new ForbidResult();

                    context.HttpContext.Request.Body.Position = 0;
                    
                }
            }
        }
    }
}
