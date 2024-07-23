using Microsoft.Extensions.Primitives;
using Project.API.Entities;

namespace Project.API.Interfaces
{
    public interface IUserService
    {
        (bool, int) CheckUserCredentials(UserCredentials userCredentials);
        Task<UserDto?> CreateUser(Dictionary<string, StringValues> body);
        Task<UserDto?> GetUserById(int id);
        Task<IEnumerable<UserDto?>> GetAllUsers();
        Task<bool> DeleteUser(int userId);
    }
}
