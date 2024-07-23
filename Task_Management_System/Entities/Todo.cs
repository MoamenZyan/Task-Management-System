using Project.API.Enums;

namespace Project.API.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public int Owner_Id { get; set; }
        public int Project_Id { get; set; }
        public required string Name { get; set; }
        public required TodoStatus Status { get; set; }
        public required DateTime Deadline { get; set; }


        public User Owner { get; set; } = null!;
        public Project Project { get; set; } = null!;
        public IEnumerable<UserTodos> AssignedUsers { get; set; } = new List<UserTodos>();
    }


    public class TodoDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Status { get; set; }
        public required DateTime Deadline { get; set; }

        public UserMinimalDto Owner { get; set; } = null!;
        public ProjectMinimalDto Project { get; set; } = null!;
        public IEnumerable<UserMinimalDto> AssignedUsers { get; set; } = new List<UserMinimalDto>();
    }

    public class TodoMinimalDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Status { get; set; }
        public required DateTime Deadline { get; set; }
    }
}
