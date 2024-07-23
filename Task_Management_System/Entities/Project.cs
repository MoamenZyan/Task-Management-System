using Project.API.Enums;

namespace Project.API.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public ProjectPrivacy Privacy { get; set; }
        public int Owner_Id { get; set; }



        public User Owner { get; set; } = null!;
        public IEnumerable<UserProjects> Users { get; set; } = new List<UserProjects>();
        public IEnumerable<Todo> Todos { get; set; } = new List<Todo>();
    }



    public class ProjectDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Privacy { get; set; }

        public UserMinimalDto Owner { get; set; } = null!;
        public IEnumerable<UserMinimalDto> Users { get ; set; } = new List<UserMinimalDto>();
        public IEnumerable<TodoMinimalDto> Todos { get; set; } = new List<TodoMinimalDto>();
    }


    public class ProjectMinimalDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Privacy { get; set; }

        public UserMinimalDto Owner { get; set; } = null!;
    }
}
