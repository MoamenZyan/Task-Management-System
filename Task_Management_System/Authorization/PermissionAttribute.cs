
using Project.API.Entities;
using Project.API.Enums;

namespace Project.API.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAttribute : Attribute
    {
        public PermissionAttribute(UsersPolicies policy)
        {
            Permission = policy;
        }
        public UsersPolicies Permission { get; }
    }
}
