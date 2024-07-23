using Microsoft.Extensions.Primitives;
using Project.API.Entities;
using Project.API.Factories;
using Project.API.Interfaces;
using Project.API.Utils;

namespace Project.API.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // Function to check user's credentials
        public (bool, int) CheckUserCredentials(UserCredentials userCredentials)
        {
            var user = _userRepository.Filter(u => u.Email == userCredentials.Email).FirstOrDefault();
            if (user == null)
                return (false, 0);

            if (BCrypt.Net.BCrypt.Verify(userCredentials.Password, user.Password))
                return (true, user.Id);
            else
                return (false, 0);
        }

        // Create user
        public async Task<UserDto?> CreateUser(Dictionary<string, StringValues> body)
        {
            User? user = await _userRepository.AddAsync(UserFactory.CreateUser(body));
            if (user == null)
                return null;

            return UserFactory.CreateUserDto(user);
        }

        // Gets all users
        public async Task<IEnumerable<UserDto?>> GetAllUsers()
        {
            IEnumerable<User?> users = await _userRepository.GetAllAsync();
            if (users == null || users.Count() == 0)
                return Enumerable.Empty<UserDto>();

            return users.Select(x => UserFactory.CreateUserDto(x!)).ToList();
        }

        // Gets user by his id
        public async Task<UserDto?> GetUserById(int id)
        {
            User? user = await _userRepository.GetById(id);
            if (user == null) return null;
            return UserFactory.CreateUserDto(user);
        }

        public async Task<bool> DeleteUser(int userId)
        {
            bool result = await _userRepository.DeleteAsync(userId);
            return result;
        }

    }
}
