using Microsoft.Extensions.Primitives;
using Project.API.Entities;
using Project.API.Factories;
using Project.API.Interfaces;
using Project.API.Strategies.EmailStrategies;
using Project.API.Strategies.SMSStrategies;
using Project.API.Utils;
namespace Project.API.Services
{
    public class ApplicationService
    {
        public readonly IUserService _userService;
        public readonly IAuthorizationPolicy _authorization;
        public readonly IProjectService _projectService;
        public readonly IUserProjectService _userProjectService;
        public readonly IUserTodosService _userTodoService;
        public readonly ITodoService _todoService;
        public readonly JwtHelper _jwtHelper;
        public readonly SendGridSettings sendGridSettings;
        public readonly NotificationService _notificationService;
        public readonly SMSSettings _smsSettings;
   

        public ApplicationService(IUserService userService,
                        IProjectService projectService,
                        JwtHelper jwtHelper,
                        IAuthorizationPolicy authorization,
                        IUserProjectService userProjectService,
                        IUserTodosService userTodoService,
                        ITodoService todoService,
                        SendGridSettings sendGridSettings,
                        NotificationService notificationService,
                        SMSSettings smsSettings)
        {
            _jwtHelper = jwtHelper;
            _userService = userService;
            _projectService = projectService;
            _userProjectService = userProjectService;
            _authorization = authorization;
            _userTodoService = userTodoService;
            _todoService = todoService;
            this.sendGridSettings = sendGridSettings;
            _notificationService = notificationService;
            _smsSettings = smsSettings;
        }

        // User login logic
        public string? Userlogin(string userId)
        {
            var token = _jwtHelper.GenerateToken(Convert.ToInt32(userId));
            return token;
        }

        // Creating User logic
        public async Task<bool> CreateUser(Dictionary<string, StringValues> body)
        {
            var result = await _userService.CreateUser(body);
            if (result != null)
            {
                _notificationService.SetNotificationStrategy(new WelcomeEmailStrategy(sendGridSettings));
                await _notificationService.Send(result.Email, result.Fname, null!);
                return true;
            }

            return false;
        }

        // Creating Project logic
        public async Task<bool> CreateProject(Dictionary<string, StringValues> body, int userId)
        {
            try
            {
                if (body == null)
                    return false;

                var result = await _projectService.CreateProject(body, userId);
                if (result != null)
                {
                    await _userProjectService.CreateUserProject(userId, result.Id, "Owner");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return false;
            }
        }

        // Adding user to project
        public async Task<bool> AddingUserToProject(Dictionary<string, StringValues> body)
        {
            try
            {
                if (body == null)
                    return false;

                int userId = Convert.ToInt32(body["UserId"]);
                int projectId = Convert.ToInt32(body["ProjectId"]);
                string role = Convert.ToString(body["Role"]);

                var result = await _userProjectService.CreateUserProject(userId, projectId, role);
                if (result)
                {
                    var user = await _userService.GetUserById(userId);
                    var project = await _projectService.GetProjectById(projectId);
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("ProjectName", project!.Name);
                    dict.Add("UserRole", body["Role"]!);

                    _notificationService.SetNotificationStrategy(new AddedToProjectEmailStrategy(sendGridSettings));
                    await _notificationService.Send(user!.Email, user.Fname, dict);


                    _notificationService.SetNotificationStrategy(new AddedToProjectSMSStrategy(_smsSettings));
                    await _notificationService.Send(user.Phone, user.Fname, dict);

                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        
        // Remove user from project
        public async Task<bool> RemovingUserFromProject(Dictionary<string, StringValues> body)
        {
            try
            {
                if (body == null)
                    return false;

                int userId = Convert.ToInt32(body["UserId"]);
                int projectId = Convert.ToInt32(body["ProjectId"]);

                var result = await _userProjectService.DeleteUserFromProject(userId, projectId);
                if (result)
                {
                    var user = await _userService.GetUserById(userId);
                    var project = await _projectService.GetProjectById(projectId);
                    _notificationService.SetNotificationStrategy(new RemovedFromProjectEmailStrategy(sendGridSettings));
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("ProjectName", project!.Name);
                    dict.Add("UserRole", body["Role"]!);
                    await _notificationService.Send(user!.Email, user.Fname, dict);


                    _notificationService.SetNotificationStrategy(new RemovedFromProjectSMSStrategy(_smsSettings));
                    dict.Clear();
                    dict.Add("ProjectName", project.Name);
                    await _notificationService.Send(user.Phone, user.Fname, dict);

                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        // Change user role in project
        public async Task<bool> ChangeUserRoleInProject(Dictionary<string, StringValues> body)
        {
           try
           {
                int userId = Convert.ToInt32(body["UserId"]);
                int projectId = Convert.ToInt32(body["ProjectId"]);
                string role = Convert.ToString(body["Role"]);

                var result = await _userProjectService.UpdateUserRoleInProject(userId, projectId, role);
                return result;
           }
           catch (Exception ex)
           {
                Console.WriteLine(ex.Message);
                return false;
           }
        }

        // Assign user to task(todo)
        public async Task<TodoDto?> CreateTask(Dictionary<string, StringValues> body, int ownerId)
        {
            var result = await _todoService.CreateTodo(body, ownerId);

            if (result != null)
                return TodoFactory.CreateTodoDto(result);
            else
                return null!;
        }

        // Update task status(Finished, InProgress, OnHold)
        public async Task<bool> UpdateTaskStatus(Dictionary<string, StringValues> body)
        {
            try
            {
                var todoId = Convert.ToInt32(body["TodoId"]);
                var status = body["Status"];

                var result = await _todoService.UpdateTodoStatus(todoId, status!);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        // Delete task from project
        public async Task<bool> DeleteTaskFromProject(int todoId)
        {
            try
            {
                var result = await _todoService.DeleteTodo(todoId);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        // Unassign user from task
        public async Task<bool> UnassignUserFromTask(Dictionary<string, StringValues> body)
        {
            try
            {
                var todoId = Convert.ToInt32(body["TodoId"]);
                var userId = Convert.ToInt32(body["UserId"]);
                var todo = await _todoService.GetTodoById(todoId);
                if (todo == null)
                    return false;

                var userInProject = await _userProjectService.CheckUserInProject(userId, todo.Project.Id);

                if (!userInProject) return false;

                var result = await _userTodoService.DeleteUserTodo(userId, todoId);
                if (result)
                {
                    var user = await _userService.GetUserById(userId);
                    _notificationService.SetNotificationStrategy(new UnassignedFromTaskEmailStrategy(sendGridSettings));
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("TodoName", todo.Name);
                    await _notificationService.Send(user!.Email, user.Fname, dict);

                    _notificationService.SetNotificationStrategy(new UnassignedFromTaskSMSStrategy(_smsSettings));
                    await _notificationService.Send(user.Phone, user.Fname, dict);

                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        // assign user from task
        public async Task<bool> AssignUserFromTask(Dictionary<string, StringValues> body)
        {
            try
            {
                var todoId = Convert.ToInt32(body["TodoId"]);
                var userId = Convert.ToInt32(body["UserId"]);

                var result = await _userTodoService.CreateUserTodo(userId, todoId);
                if (result)
                {
                    var user = await _userService.GetUserById(userId);
                    var todo = await _todoService.GetTodoById(todoId);
                    _notificationService.SetNotificationStrategy(new AssignedFromTaskEmailStrategy(sendGridSettings));
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("TodoName", todo!.Name);
                    dict.Add("Deadline", todo.Deadline.ToString());
                    await _notificationService.Send(user!.Email, user.Fname, dict);

                    _notificationService.SetNotificationStrategy(new AssignedToTaskSMSStrategy(_smsSettings));
                    await _notificationService.Send(user.Phone, user.Fname, dict);

                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
