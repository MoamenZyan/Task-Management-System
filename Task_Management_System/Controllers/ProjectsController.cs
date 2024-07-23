using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Project.API.Entities;
using Project.API.Interfaces;
using Project.API.Services;
using Swashbuckle.AspNetCore.Annotations;
using Project.API.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Project.API.Controllers
{
    [ApiController]
    [Route("/api/v1/projects")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectsService;
        private readonly ApplicationService _applicationService;

        public ProjectsController(IProjectService projectsService, ApplicationService applicationService)
        {
            _projectsService = projectsService;
            _applicationService = applicationService;
        }

        /// <summary>
        /// Endpoint to get all projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        // Get all public projects
        public async Task<IActionResult> GetAllProjects()
        {
            IEnumerable<ProjectDto?> projects = await _projectsService.GetAllProjects();
            return Ok(projects);
        }


        /// <summary>
        /// Endpoint to get specific project by id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        // Get specific project by id
        public async Task<IActionResult> GetProjectById(int id)
        {
            ProjectDto? project = await _projectsService.GetProjectById(id);
            return Ok(project);
        }


        /// <summary>
        /// Endpoint to create project
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        // Create project action
        public async Task<IActionResult> CreateProject()
        {
            var body = await new FormReader(Request.Body).ReadFormAsync();

            bool result = await _applicationService.CreateProject(body, Convert.ToInt32(HttpContext.Items["userId"]));
            if (result)
                return Ok(new { status = true, message = "project created !" });
            else
                return BadRequest(new { status = false, message = "there is problem when creating project" });
        }

        /// <summary>
        /// Adds user to a project.
        /// </summary>
        /// <returns>result of the operation</returns>
        [HttpPost]
        [Route("user")]
        [Permission(Enums.UsersPolicies.AddTaskToProject)]
        [TypeFilter(typeof(PermissionBasedAuthorizationForProjectFilter))]
        // High authority user add user to project
        public async Task<IActionResult> AddUserToProject()
        {
            var body = await new FormReader(Request.Body).ReadFormAsync();

            bool result = await _applicationService.AddingUserToProject(body);
            if (result)
                return Ok(new { status = true, message = "user added successfully !" });
            else
                return BadRequest(new { status = false, message = "there is problem when adding user to project" });
        }

        [HttpDelete]
        [Route("user")]
        [Permission(Enums.UsersPolicies.DeleteUserFromProject)]
        [TypeFilter(typeof(PermissionBasedAuthorizationForProjectFilter))]
        // High authority user delete other user from project
        public async Task<IActionResult> DeleteUserFromProject()
        {
            var body = await new FormReader(Request.Body).ReadFormAsync();

            bool result = await _applicationService.RemovingUserFromProject(body);

            if (result)
                return Ok(new { status = true, message = "user removed from project successfully !" });
            else
                return BadRequest(new { status = false, message = "there is problem when adding user to project" });
        }

        [HttpDelete]
        [Route("currentUser")]
        // Current leave project
        public async Task<IActionResult> LeaveProject()
        {
            var body = await new FormReader(Request.Body).ReadFormAsync();
            if (Convert.ToInt32(body["UserId"]) != Convert.ToInt32(Request.HttpContext.Items["userId"]))
                return BadRequest(new { status = false, message = "error in leaving project" });

            bool result = await _applicationService.RemovingUserFromProject(body);

            if (result)
                return Ok(new { status = true, message = "you leaved from project successfully !" });
            else
                return BadRequest(new { status = false, message = "there is problem when leaving from project" });
        }


        [HttpPatch]
        [Route("user")]
        [Permission(Enums.UsersPolicies.UpdateUserRole)]
        [TypeFilter(typeof(PermissionBasedAuthorizationForProjectFilter))]
        // High authority user update user role in project
        public async Task<IActionResult> UpdateUserRoleInProject()
        {
            var body = await new FormReader(Request.Body).ReadFormAsync();
            if (body == null)
                return BadRequest(new { status = false, message = "body is null" });
            var result = await _applicationService.ChangeUserRoleInProject(body);
            if (result)
                return Ok(new { status = true, message = "user role in a project is updated !" });
            else
                return new JsonResult(new { status = false, message = "error in updating user role" }) { StatusCode = 500 };
        }
    }
}
