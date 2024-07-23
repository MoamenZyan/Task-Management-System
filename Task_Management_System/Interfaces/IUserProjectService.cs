using Project.API.Enums;

namespace Project.API.Interfaces
{
    public interface IUserProjectService
    {
        Task<bool> CreateUserProject(int userId, int projectId, string Role);
        Task<bool> DeleteUserFromProject(int userId, int projectId);
        Task<bool> UpdateUserRoleInProject(int userId, int projectId, string NewRole);
        Task<UserRole?> GetUserRoleInProject(int userId, int projectId);
        Task<bool> CheckUserInProject(int userId, int projectId);
    }
}
