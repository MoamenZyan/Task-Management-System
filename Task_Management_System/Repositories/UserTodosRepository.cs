using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore;
using Project.API.Entities;
using Project.API.Interfaces;
using Task_Management_System.Data;

namespace Project.API.Repositories
{
    public class UserTodosRepository : IJunctionRepository<UserTodos>
    {
        private readonly ApplicationDbContext _context;
        public UserTodosRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserTodos?> AddAsync(UserTodos entity)
        {
            try
            {
                await _context.UserTodos.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null!;
            }
        }

        public async Task<bool> DeleteAsync(int userId, int entityId)
        {
            try
            {
                var result = await GetById(userId, entityId);
                if (result == null)
                    return true;

                _context.UserTodos.Remove(result);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public IEnumerable<UserTodos?> Filter(Func<UserTodos, bool> func)
        {
            return _context.UserTodos.Where(func).ToList();
        }

        public async Task<UserTodos?> GetById(int userId, int entityId)
        {
            try
            {
                var result = await _context.UserTodos.FirstOrDefaultAsync(x => x.User_Id == userId &&
                                x.Todo_Id == entityId);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null!;
            }
        }

        public async Task<bool> UpdateAsync(UserTodos entity)
        {
            try
            {
                _context.UserTodos.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
