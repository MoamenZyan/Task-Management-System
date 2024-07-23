using Microsoft.Extensions.Primitives;
using Project.API.Entities;
using Ganss.Xss;
using Project.API.Enums;

namespace Project.API.Factories
{
    public class ProjectFactory
    {
        // Factory method to create Project object
        public static Entities.Project CreateProject(Dictionary<string, StringValues> body, int id)
        {
            HtmlSanitizer sanitizer = new HtmlSanitizer();

            Entities.Project project = new Entities.Project
            {
                Name = sanitizer.Sanitize(body["Name"]!),
                Privacy = Enum.Parse<ProjectPrivacy>(body["Privacy"]!),
                Owner_Id = id
            };

            return project;
        }

        // Factory method to create ProjectDto object out of project
        public static ProjectDto CreateProjectDto(Entities.Project project)
        {
            ProjectDto projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Privacy = project.Privacy.ToString(),
                Owner = UserFactory.CreateUserMinimalDto(project.Owner),
                Users = project.Users.Where(x => x.User.Id != project.Owner.Id).Select(x => UserFactory.CreateUserMinimalDto(x.User)).ToList(),
                Todos = project.Todos.Select(x => TodoFactory.CreateTodoMinimalDto(x)).ToList()
            };

            return projectDto;
        }

        // Factory method to create ProjectMinimalDto object out of project
        public static ProjectMinimalDto CreateProjectMinimalDto(Entities.Project project)
        {
            ProjectMinimalDto projectMinimalDto = new ProjectMinimalDto
            {
                Id = project.Id,
                Name = project.Name,
                Privacy = project.Privacy.ToString(),
                Owner = UserFactory.CreateUserMinimalDto(project.Owner)
            };

            return projectMinimalDto;
        }
    }
}
