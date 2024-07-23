using Microsoft.Extensions.Primitives;
using Project.API.Entities;
using Project.API.Enums;
using Project.API.Factories;
using Project.API.Interfaces;

namespace Project.API.Services
{
    public class TodoService : ITodoService
    {
        private readonly IRepository<Todo> _todoRepository;
        public TodoService(IRepository<Todo> todoRepository)
        {
            _todoRepository = todoRepository;
        }
        // Create Todo 
        public async Task<Todo?> CreateTodo(Dictionary<string, StringValues> body, int ownerId)
        {
            var todo = TodoFactory.CreateTodo(body, ownerId);
            var result = await _todoRepository.AddAsync(todo);
            return result;
        }
        // Delete todo from project
        public async Task<bool> DeleteTodo(int todoId)
        {
            var result = await _todoRepository.DeleteAsync(todoId);
            return result;
        }

        // Get todo by id
        public async Task<TodoDto?> GetTodoById(int todoId)
        {
            var todo = await _todoRepository.GetById(todoId);
            if (todo == null)
                return null;

            return TodoFactory.CreateTodoDto(todo);
        }

        // Update todo status
        public async Task<bool> UpdateTodoStatus(int todoId, string status)
        {
            var todo = await _todoRepository.GetById(todoId);
            if (todo == null) return false;

            todo.Status = Enum.Parse<TodoStatus>(status);
            var result = await _todoRepository.UpdateAsync(todo);
            return result;
        }
    }
}
