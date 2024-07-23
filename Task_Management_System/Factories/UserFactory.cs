using Ganss.Xss;
using Microsoft.Extensions.Primitives;
using Project.API.Entities;
using BCrypt.Net;

namespace Project.API.Factories
{
    public class UserFactory
    {
        // Factory method to create User object
        public static User CreateUser(Dictionary<string, StringValues> body)
        {
            HtmlSanitizer sanitizer = new HtmlSanitizer();

            User user = new User
            {
                Fname = sanitizer.Sanitize(body["Fname"]!),
                Lname = sanitizer.Sanitize(body["Lname"]!),
                Email = sanitizer.Sanitize(body["Email"]!),
                Password = BCrypt.Net.BCrypt.HashPassword(sanitizer.Sanitize(body["Password"]!), 10),
                Phone = sanitizer.Sanitize(body["Phone"]!),
            };
            return user;
        }

        // Factory method to create UserDto object out of User
        public static UserDto CreateUserDto(User user)
        {
            UserDto userDto = new UserDto
            {
                Id = user.Id,
                Fname = user.Fname,
                Lname = user.Lname,
                Email = user.Email,
                Phone = user.Phone,
                JoinedProjects = user.JoinedProjects.Where(x => x.Project.Owner.Id != user.Id).Select(x => ProjectFactory.CreateProjectMinimalDto(x.Project)).ToList(),
                OwnedProjects = user.OwnedProjects.Select(x => ProjectFactory.CreateProjectMinimalDto(x)).ToList(),
            };

            return userDto;
        }

        // Factory method to create UserMinimalDto object out of User
        public static UserMinimalDto CreateUserMinimalDto(User user)
        {
            UserMinimalDto userMinimalDto = new UserMinimalDto
            {
                Id = user.Id,
                Fname = user.Fname,
                Lname = user.Lname,
                Email = user.Email,
                Phone = user.Phone,
            };

            return userMinimalDto;
        }
    }
}
