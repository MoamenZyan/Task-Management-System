using Project.API.Entities;

namespace Project.API.Factories
{
    public class UserTodosFactory
    {
        public static UserTodos CreateUserTodo(int userId, int todoId)
        {
            UserTodos userTodos = new UserTodos
            {
                User_Id = userId,
                Todo_Id = todoId,
            };
            return userTodos;
        }
    }
}
