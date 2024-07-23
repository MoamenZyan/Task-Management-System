using Project.API.Entities;
using Project.API.Factories;
using Project.API.Interfaces;
using Project.API.Enums;

namespace Project.API.Services
{
    public class UserProjectService : IUserProjectService
    {
        private readonly IJunctionRepository<UserProjects> _userProjectRepository;

        public UserProjectService(IJunctionRepository<UserProjects> userProjectRepository)
        {
            _userProjectRepository = userProjectRepository;
        }

        // Create UserProject object and save it to DB
        public async Task<bool> CreateUserProject(int userId, int projectId, string Role)
        {
            var checkExists = await _userProjectRepository.GetById(userId, projectId);

            if ( checkExists != null )
                return false;
            
            UserProjects userProjects = UserProjectsFactory.CreateUserProject(userId, projectId, Enum.Parse<UserRole>(Role));
            var result = await _userProjectRepository.AddAsync(userProjects);
            if (result != null)
                return true;
            return false;
        }

        // Delete UserProject object from DB
        public Task<bool> DeleteUserFromProject(int userId, int projectId)
        {
            var result = _userProjectRepository.DeleteAsync(userId, projectId);
            return result;
        }

        // Update UserProject object
        public async Task<bool> UpdateUserRoleInProject(int userId, int projectId, string NewRole)
        {
            var entity = await _userProjectRepository.GetById(userId, projectId);
            if (entity == null)
                return false;

            entity.Role = Enum.Parse<UserRole>(NewRole);
            var result = await _userProjectRepository.UpdateAsync(entity);
            return result;
        }

        // Getting user role in a project
        public async Task<UserRole?> GetUserRoleInProject(int userId, int projectId)
        {
            var entity = await _userProjectRepository.GetById(userId, projectId);
            return entity?.Role;
        }
        // Check user in project
        public async Task<bool> CheckUserInProject(int userId, int projectId)
        {
            var result = await _userProjectRepository.GetById(userId, projectId);
            return result != null;
        }
    }
}
