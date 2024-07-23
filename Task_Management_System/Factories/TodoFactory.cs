using Ganss.Xss;
using Microsoft.Extensions.Primitives;
using Project.API.Entities;

namespace Project.API.Factories
{
    public class TodoFactory
    {
        // Factory to create Todo object
        public static Todo CreateTodo(Dictionary<string, StringValues> body, int id)
        {
            HtmlSanitizer sanitizer = new HtmlSanitizer();

            Todo todo = new Todo
            {
                Name = sanitizer.Sanitize(body["Name"]!),
                Deadline = Convert.ToDateTime(body["Deadline"]),
                Project_Id = Convert.ToInt16(body["ProjectId"]),
                Owner_Id = id,
                Status = Enums.TodoStatus.OnHold,
            };
            return todo;
        }

        // Factory to create TodoDto object out of Todo
        public static TodoDto CreateTodoDto(Todo todo)
        {

            TodoDto todoDto = new TodoDto
            {
                Id = todo.Id,
                Name = todo.Name,
                Status = todo.Status.ToString(),
                Project = ProjectFactory.CreateProjectMinimalDto(todo.Project),
                Owner = UserFactory.CreateUserMinimalDto(todo.Owner),
                AssignedUsers = todo.AssignedUsers.Select(x => UserFactory.CreateUserMinimalDto(x.User)).ToList(),
                Deadline = todo.Deadline
            };
            return todoDto;
        }

        // Factory to create TodoMinimalDto object out of Todo
        public static TodoMinimalDto CreateTodoMinimalDto(Todo todo)
        {
            TodoMinimalDto todoMinimalDto = new TodoMinimalDto
            {
                Name = todo.Name,
                Status = todo.Status.ToString(),
                Deadline = todo.Deadline
            };
            return todoMinimalDto;
        }
    }
}
