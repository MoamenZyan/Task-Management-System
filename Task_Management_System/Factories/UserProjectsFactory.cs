using Project.API.Entities;
using Project.API.Enums;

namespace Project.API.Factories
{
    public class UserProjectsFactory
    {
        public static UserProjects CreateUserProject(int userId, int projectId, UserRole role)
        {
            UserProjects userProject = new UserProjects
            { 
                User_Id = userId,
                Project_Id = projectId,
                Role = role
            };
            return userProject;
        }
    }
}
