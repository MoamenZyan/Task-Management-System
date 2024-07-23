using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.API.Interfaces;
using Project.API.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using Project.API.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Project.API.Controllers
{
    [ApiController]
    [Route("/api/v1/todos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TodosController : ControllerBase
    {
        private readonly ApplicationService _applicationService;
        private readonly ITodoService _todoService;
        public TodosController(ApplicationService applicationService, ITodoService todoService)
        {
            _applicationService = applicationService;
            _todoService = todoService;
        }

        [HttpGet]
        [Route("{todoId}")]
        public async Task<IActionResult> GetTodoById(int todoId)
        {
            var todo = await _todoService.GetTodoById(todoId);
            if (todo == null)
                return NotFound(new {status = false, message = "todo is not found"});
            else
                return Ok(new {status = true, todo = todo});
        }

        [HttpPost]
        [Route("")]
        [Permission(Enums.UsersPolicies.AddTaskToProject)]
        [TypeFilter(typeof(PermissionBasedAuthorizationForProjectFilter))]
        public async Task<IActionResult> CreateTodo()
        {
            var body = await new FormReader(Request.Body).ReadFormAsync();

            if (body == null)
                return BadRequest(new { status = false, message = "body is null" });

            var userId = Convert.ToInt16(Request.HttpContext.Items["userId"]);

            var result = await _applicationService.CreateTask(body, userId);

            if (result is null)
                return new JsonResult(new { status = false, message = "error in creating task" }) { StatusCode = 500 };
            else
                return Ok(new { status = true, todo = result });
        }

        [HttpDelete]
        [Route("{todoId}")]
        [Permission(Enums.UsersPolicies.DeleteTaskFromProject)]
        [TypeFilter(typeof(PermissionBasedAuthorizationForProjectFilter))]
        public async Task<IActionResult> DeleteTodo(int todoId)
        {
            var result = await _todoService.DeleteTodo(todoId);

            if (result == false)
                return new JsonResult(new { status = false, message = "error in deleting task" }) { StatusCode = 500 };
            else
                return Ok(new { status = true, message = "todo deleted successfully"});
        }

        [HttpPatch]
        [Route("")]
        [Permission(Enums.UsersPolicies.UpdateTask)]
        [TypeFilter(typeof(PermissionBasedAuthorizationForTodoFilter))]
        public async Task<IActionResult> UpdateTodoStatus()
        {
            var body = await new FormReader(Request.Body).ReadFormAsync();

            var result = await _applicationService.UpdateTaskStatus(body);

            if (result == false)
                return new JsonResult(new { status = false, message = "error in updating task" }) { StatusCode = 500 };
            else
                return Ok(new { status = true, message = "todo updated successfully" });
        }

        [HttpPost]
        [Route("user")]
        [Permission(Enums.UsersPolicies.AssignUserToTask)]
        [TypeFilter(typeof(PermissionBasedAuthorizationForProjectFilter))]
        public async Task<IActionResult> AssignUserToTask()
        {
            var body = await new FormReader(Request.Body).ReadFormAsync();
            var result = await _applicationService.AssignUserFromTask(body);
            if (result)
                return Ok(new { status = true, message = "user assigned to task successfully" });
            else 
                return new JsonResult(new {status = false, message = "error in assigning user to task"}) { StatusCode = 500 };
        }

        [HttpDelete]
        [Route("user")]
        [Permission(Enums.UsersPolicies.UnassignUserFromTask)]
        [TypeFilter(typeof(PermissionBasedAuthorizationForProjectFilter))]
        public async Task<IActionResult> UnassignUserToTask()
        {
            var body = await new FormReader(Request.Body).ReadFormAsync();
            var result = await _applicationService.UnassignUserFromTask(body);
            if (result)
                return Ok(new { status = true, message = "user unassigned from task successfully" });
            else
                return new JsonResult(new { status = false, message = "error in unassigning user from task" }) { StatusCode = 500 };
        }
    }
}
