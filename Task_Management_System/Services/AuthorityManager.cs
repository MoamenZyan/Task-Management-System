using Project.API.Enums;
using Project.API.Interfaces;

namespace Project.API.Services
{
    public class AuthorityManager : IAuthorizationPolicy
    {
        // Mapping each role to what cant it do in policies
        private readonly Dictionary<UserRole, List<UsersPolicies>> _rolePolicies = new Dictionary<UserRole, List<UsersPolicies>>
        {
            { UserRole.Owner, new List<UsersPolicies> {} },
            { UserRole.Admin, new List<UsersPolicies> { UsersPolicies.DeleteProject } },
            { UserRole.TeamLeader, new List<UsersPolicies> { UsersPolicies.DeleteProject, UsersPolicies.UpdateProjectInfo, UsersPolicies.DeleteUserFromProject, UsersPolicies.AddUserToProject } },


            { UserRole.Member, new List<UsersPolicies> { UsersPolicies.DeleteProject, UsersPolicies.AddUserToProject, UsersPolicies.DeleteUserFromProject,
            UsersPolicies.UpdateProjectInfo, UsersPolicies.AssignUserToTask, UsersPolicies.UnassignUserFromTask, UsersPolicies.DeleteTaskFromProject,
            UsersPolicies.AddTaskToProject, UsersPolicies.UpdateUserRole} }
        };

        public bool CheckAuthorizationOnAction(UsersPolicies policy, UserRole role)
        {
            return _rolePolicies.TryGetValue(role, out var policies) && !policies.Contains(policy);
        }
    }
}
