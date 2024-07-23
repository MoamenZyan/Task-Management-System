using Microsoft.EntityFrameworkCore;
using Project.API.Entities;
using Project.API.Interfaces;
using Task_Management_System.Data;
using Newtonsoft.Json;
using Project.API.Utils;

namespace Project.API.Repositories
{
    public class UserRepository : IRepository<User>
    {

        private readonly ApplicationDbContext _context;
        private readonly ICacheService _redis;
        public UserRepository(ApplicationDbContext context, ICacheService redis)
        {
            _context = context;
            _redis = redis;
        }
        public async Task<User?> AddAsync(User entity)
        {
            try
            {
                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();
                var user = await GetById(entity.Id);
                await _redis.Set($"user-{entity.Id}", JsonConvert.SerializeObject(user, JsonDefaultSettings.jsonSerializerSettings));
                return entity;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return null!;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                User? user = await GetById(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    await _redis.Del($"user-{id}");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public IEnumerable<User?> Filter(Func<User, bool> func)
        {
            return _context.Users.Where(func).ToList();
        }

        public async Task<IEnumerable<User?>> GetAllAsync()
        {
            var fromCache = await _redis.Get("users");
            if (fromCache == null)
            {
                var fromDB = await _context.Users
                                    .Include(x => x.OwnedProjects)
                                    .Include(x => x.AssignedTodos)
                                        .ThenInclude(x => x.Todo)
                                    .Include(x => x.JoinedProjects)
                                        .ThenInclude(x => x.Project)
                                    .Include(x => x.JoinedProjects)
                                        .ThenInclude(x => x.Project.Owner)
                                    .Include(x => x.OwnedTodos)
                                    .ToListAsync();

                if (fromDB != null)
                    await _redis.Set("users", JsonConvert.SerializeObject(fromDB, JsonDefaultSettings.jsonSerializerSettings));

                return fromDB!;
            }

            return JsonConvert.DeserializeObject<IEnumerable<User>>(fromCache)!;
        }

        public async Task<User?> GetById(int id)
        {
            var fromCache = await _redis.Get($"user-{id}");
            if (fromCache == null)
            {
                var fromDB = await _context.Users
                                    .Include(x => x.OwnedProjects)
                                    .Include(x => x.AssignedTodos)
                                        .ThenInclude(x => x.Todo)
                                    .Include(x => x.JoinedProjects)
                                        .ThenInclude(x => x.Project)
                                    .Include(x => x.JoinedProjects)
                                        .ThenInclude(x => x.Project.Owner)
                                    .Include(x => x.OwnedTodos)
                                    .FirstOrDefaultAsync(x => x.Id == id)!;

                if (fromDB != null)
                   await _redis.Set($"user-{id}", JsonConvert.SerializeObject(fromDB, JsonDefaultSettings.jsonSerializerSettings));
                return fromDB!;
            }
            return JsonConvert.DeserializeObject<User>(fromCache)!;
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            try
            {
                _context.Users.Update(entity);
                await _context.SaveChangesAsync();
                var user = await GetById(entity.Id);
                await _redis.Set($"user-{entity.Id}", JsonConvert.SerializeObject(user, JsonDefaultSettings.jsonSerializerSettings));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
