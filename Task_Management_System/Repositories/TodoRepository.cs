using Project.API.Interfaces;
using Project.API.Entities;
using Task_Management_System.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project.API.Utils;

namespace Project.API.Repositories
{
    public class TodoRepository : IRepository<Todo>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICacheService _redis;
        public TodoRepository(ApplicationDbContext context, ICacheService redis)
        {
            _context = context;
            _redis = redis;
        }

        public async Task<Todo?> AddAsync(Todo entity)
        {
            try
            {
                await _context.Todos.AddAsync(entity);
                await _context.SaveChangesAsync();
                var todo = await GetById(entity.Id);
                await _redis.Set($"todo-{todo!.Id}", JsonConvert.SerializeObject(todo, JsonDefaultSettings.jsonSerializerSettings));
                return todo;
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
                Todo? todo = await GetById(id);
                if (todo != null)
                {
                    _context.Todos.Remove(todo);
                    await _context.SaveChangesAsync();
                    await _redis.Del($"todo-{todo.Id}");
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public IEnumerable<Todo?> Filter(Func<Todo, bool> func)
        {
            return _context.Todos.Where(func).ToList();
        }

        public async Task<IEnumerable<Todo?>> GetAllAsync()
        {
            var fromCache = await _redis.Get($"todos");
            if (fromCache == null)
            {
                var fromDB = await _context.Todos
                                    .Include(x => x.Owner)
                                    .Include(x => x.AssignedUsers)
                                        .ThenInclude(x => x.User)
                                    .Include(x => x.Project)
                                        .Include(x => x.Owner)
                                    .ToListAsync();

                if (fromDB != null)
                    await _redis.Set($"todos", JsonConvert.SerializeObject(fromDB, JsonDefaultSettings.jsonSerializerSettings));
                return fromDB!;
            }

            return JsonConvert.DeserializeObject<IEnumerable<Todo>>(fromCache)!;
        }

        public async Task<Todo?> GetById(int id)
        {
            var fromCache = await _redis.Get($"todo-{id}");
            if (fromCache == null)
            {
                var fromDB = await _context.Todos
                        .Include(x => x.Owner)
                        .Include(x => x.AssignedUsers)
                            .ThenInclude(x => x.User)
                        .Include(x => x.Project)
                            .Include(x => x.Owner)
                        .FirstOrDefaultAsync(x => x.Id == id);

                if (fromDB != null)
                    await _redis.Set($"todo-{id}", JsonConvert.SerializeObject(fromDB, JsonDefaultSettings.jsonSerializerSettings));
                return fromDB!;
            }

            return JsonConvert.DeserializeObject<Todo>(fromCache)!;
        }

        public async Task<bool> UpdateAsync(Todo entity)
        {
            try
            {
                _context.Todos.Update(entity);
                await _context.SaveChangesAsync();
                var todo = await GetById(entity.Id);
                await _redis.Set($"todo-{todo!.Id}", JsonConvert.SerializeObject(todo, JsonDefaultSettings.jsonSerializerSettings));
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
