namespace Project.API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Fname { get; set; }
        public required string Lname { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Phone { get; set; }


        public IEnumerable<Project> OwnedProjects { get; set; } = new List<Project>();
        public IEnumerable<Todo> OwnedTodos { get; set; } = new List<Todo>();

        public IEnumerable<UserProjects> JoinedProjects { get; set; } = new List<UserProjects>();
        public IEnumerable<UserTodos> AssignedTodos { get; set; } = new List<UserTodos>();

    }


    public class UserDto
    {
        public int Id { get; set; }
        public required string Fname { get; set; }
        public required string Lname { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

        public IEnumerable<ProjectMinimalDto> OwnedProjects { get; set; } = new List<ProjectMinimalDto>();
        public IEnumerable<ProjectMinimalDto> JoinedProjects { get; set; } = new List<ProjectMinimalDto>();
    }

    public class UserMinimalDto
    {
        public int Id { get; set; }
        public required string Fname { get; set; }
        public required string Lname { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
    }
}
