using Project.API.Interfaces;
using Project.API.Entities;
using Project.API.Factories;

namespace Project.API.Services
{
    public class UserTodosService : IUserTodosService
    {
        private readonly IJunctionRepository<UserTodos> _userTodosRepository;
        public UserTodosService(IJunctionRepository<UserTodos> userTodosRepository)
        {
            _userTodosRepository = userTodosRepository;
        }
        // Create UserTodo object and save to DB
        public async Task<bool> CreateUserTodo(int userId, int todoId)
        {
            UserTodos userTodo = UserTodosFactory.CreateUserTodo(userId, todoId);
            var result = await _userTodosRepository.AddAsync(userTodo);
            if (result is not null)
                return true;
            else
                return false;
        }

        // Delete UserTodo object from DB
        public async Task<bool> DeleteUserTodo(int userId, int todoId)
        {
            var result = await _userTodosRepository.DeleteAsync(userId, todoId);
            return result;
        }

        // Check if user is assigned on todo in project or not
        public async Task<bool> IsAssignedOnTodo(int userId, int todoId)
        {
            var result = await _userTodosRepository.GetById(userId, todoId);

            if (result is null)
                return false;

            return true;
        }
    }
}
