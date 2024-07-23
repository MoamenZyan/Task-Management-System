namespace Project.API.Interfaces
{
    public interface IUserTodosService
    {
        Task<bool> CreateUserTodo(int userId, int todoId);
        Task<bool> DeleteUserTodo(int userId, int todoId);
        Task<bool> IsAssignedOnTodo(int userId, int todoId);
    }
}
