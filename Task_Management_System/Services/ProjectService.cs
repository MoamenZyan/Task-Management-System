using Microsoft.Extensions.Primitives;
using Project.API.Entities;
using Project.API.Factories;
using Project.API.Interfaces;
using Project.API.Utils;

namespace Project.API.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Entities.Project> _projectRepository;
        private readonly IJunctionRepository<UserProjects> _userProjectsRepository;
        private readonly JwtHelper _jwtHelper;
        public ProjectService(IRepository<Entities.Project> projectRepository,
                                JwtHelper jwtHelper, IJunctionRepository<UserProjects> userProjectsRepository)
        {
            _projectRepository = projectRepository;
            _jwtHelper = jwtHelper;
            _userProjectsRepository = userProjectsRepository;
        }

        public async Task<ProjectDto?> CreateProject(Dictionary<string, StringValues> body, int ownerId)
        {
            Entities.Project? project = await _projectRepository.AddAsync(ProjectFactory.CreateProject(body, ownerId));
            if (project == null)
                return null;
            return ProjectFactory.CreateProjectDto(project);
        }

        public async Task<IEnumerable<ProjectDto?>> GetAllProjects()
        {
            IEnumerable<Entities.Project?> projects = await _projectRepository.GetAllAsync();
            if (projects.Count() == 0)
                return Enumerable.Empty<ProjectDto?>();

            return projects.Select(x => ProjectFactory.CreateProjectDto(x!)).ToList();
        }

        public async Task<ProjectDto?> GetProjectById(int id)
        {
            Entities.Project? project = await _projectRepository.GetById(id);
            if (project == null)
                return null;

            return ProjectFactory.CreateProjectDto(project);
        }
    }
}
