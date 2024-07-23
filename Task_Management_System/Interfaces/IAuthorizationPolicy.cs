using Project.API.Enums;

namespace Project.API.Interfaces
{
    public interface IAuthorizationPolicy
    {
       bool CheckAuthorizationOnAction(UsersPolicies policy, UserRole role);
    }
}
