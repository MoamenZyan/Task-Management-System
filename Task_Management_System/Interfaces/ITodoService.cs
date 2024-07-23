using Microsoft.Extensions.Primitives;
using Project.API.Entities;
namespace Project.API.Interfaces
{
    public interface ITodoService
    {
        Task<Todo?> CreateTodo(Dictionary<string, StringValues> body, int ownerId);
        Task<TodoDto?> GetTodoById(int todoId);
        Task<bool> DeleteTodo(int todoId);
        Task<bool> UpdateTodoStatus(int todoId, string status);
    }
}
