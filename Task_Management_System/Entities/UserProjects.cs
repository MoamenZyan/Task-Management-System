using Project.API.Enums;

namespace Project.API.Entities
{
    public class UserProjects
    {
        public int User_Id { get; set; }
        public int Project_Id { get; set; }

        public UserRole Role { get; set; }

        public User User { get; set; } = null!;
        public Project Project { get; set; } = null!;

    }
}
