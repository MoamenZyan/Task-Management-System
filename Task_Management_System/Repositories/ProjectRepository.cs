using Project.API.Interfaces;
using Project.API.Entities;
using Task_Management_System.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project.API.Utils;

namespace Project.API.Repositories
{
    public class ProjectRepository : IRepository<Entities.Project>
    {
        private readonly ICacheService _redis;
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context, ICacheService redis)
        {
            _redis = redis;
            _context = context;
        }

        public async Task<Entities.Project?> AddAsync(Entities.Project entity)
        {
            try
            {
                await _context.Projects.AddAsync(entity);
                await _context.SaveChangesAsync();
                var project = await GetById(entity.Id);
                await _redis.Set($"project-{project!.Id}", JsonConvert.SerializeObject(project, JsonDefaultSettings.jsonSerializerSettings));
                return project;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return null!;
            }
        }
        public async Task<Entities.Project?> GetById(int id)
        {
            var fromCache = await _redis.Get($"project-{id}");
            if (fromCache == null)
            {
                var fromDB = await _context.Projects
                                .Include(x => x.Owner)
                                .Include(x => x.Users)
                                    .ThenInclude(x => x.User)
                                .Include(x => x.Todos)
                                    .ThenInclude(x => x.Owner)
                                .FirstOrDefaultAsync(x => x.Id == id)!;

                if (fromDB != null)
                {
                    await _redis.Set($"project-{id}", JsonConvert.SerializeObject(fromDB, JsonDefaultSettings.jsonSerializerSettings));
                }
                return fromDB;
            }
            return JsonConvert.DeserializeObject<Entities.Project>(fromCache);
        }

        public async Task<IEnumerable<Entities.Project?>> GetAllAsync()
        {
            var fromCache = await _redis.Get($"projects");
            if (fromCache == null)
            {
                var fromDB = await _context.Projects
                                .Include(x => x.Owner)
                                .Include(x => x.Users)
                                    .ThenInclude(x => x.User)
                                .Include(x => x.Todos)
                                    .ThenInclude(x => x.Owner)
                                .ToListAsync();

                if (fromDB != null)
                {
                    await _redis.Set($"projects", JsonConvert.SerializeObject(fromDB, JsonDefaultSettings.jsonSerializerSettings));
                }
                return fromDB!;
            }
            return JsonConvert.DeserializeObject<IEnumerable<Entities.Project>>(fromCache)!;
        }


        public async Task<bool> UpdateAsync(Entities.Project entity)
        {
            try
            {
                _context.Projects.Update(entity);
                await _context.SaveChangesAsync();
                var project = await GetById(entity.Id);
                await _redis.Set($"project-{project!.Id}", JsonConvert.SerializeObject(project, JsonDefaultSettings.jsonSerializerSettings));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                Entities.Project? project = await GetById(id);
                if (project != null)
                {
                    _context.Projects.Remove(project);
                    await _redis.Del($"project-{id}");
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public IEnumerable<Entities.Project?> Filter(Func<Entities.Project, bool> func)
        {
            return _context.Projects
                                .Include(x => x.Owner)
                                .Include(x => x.Users)
                                    .ThenInclude(x => x.User)
                                .Include(x => x.Todos)
                                    .ThenInclude(x => x.Owner)
                                .Where(func)
                                .ToList();
        }
    }
}
