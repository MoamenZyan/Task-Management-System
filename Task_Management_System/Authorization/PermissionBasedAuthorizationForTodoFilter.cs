using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Project.API.Enums;
using Project.API.Interfaces;
using Project.API.Services;
using System.Security.Claims;

namespace Project.API.Authorization
{
    public class PermissionBasedAuthorizationForTodoFilter(ITodoService todoService, IUserProjectService userProjectService,
                IAuthorizationPolicy authorizationPolicy, IUserTodosService userTodosService) : IAsyncAuthorizationFilter
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
                    var todoId = Convert.ToInt16(body["TodoId"]);
                    var todo = await todoService.GetTodoById(todoId);
                    if (todo is null)
                        context.Result = new NotFoundResult();

                    var role = await userProjectService.GetUserRoleInProject(userId, todo!.Project.Id);
                    Console.WriteLine(userId + " " + todo.Id);
                    var isAssgiend = await userTodosService.IsAssignedOnTodo(userId, todo.Id);

                    if (role is null)
                        context.Result = new ForbidResult();

                    if (!authorizationPolicy.CheckAuthorizationOnAction(permission.Permission, (UserRole)role!))
                        context.Result = new ForbidResult();

                    if (role == Enums.UserRole.Member && !isAssgiend)
                        context.Result = new ForbidResult();
                    

                    context.HttpContext.Request.Body.Position = 0;

                }
            }
        }
    }
}
