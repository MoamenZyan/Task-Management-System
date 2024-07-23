using Microsoft.Extensions.Primitives;
using Project.API.Entities;

namespace Project.API.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDto?> CreateProject(Dictionary<string, StringValues> body, int ownerId);
        Task<ProjectDto?> GetProjectById(int id);
        Task<IEnumerable<ProjectDto?>> GetAllProjects();
    }
}
