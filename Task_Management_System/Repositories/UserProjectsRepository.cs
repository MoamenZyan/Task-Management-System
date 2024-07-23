using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore;
using Project.API.Entities;
using Project.API.Interfaces;
using Task_Management_System.Data;

namespace Project.API.Repositories
{
    public class UserProjectsRepository : IJunctionRepository<UserProjects>
    {
        public readonly ApplicationDbContext _context;

        public UserProjectsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProjects?> AddAsync(UserProjects entity)
        {
           try
           {
                await _context.UserProjects.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
           }
           catch (Exception ex)
           {
                Console.WriteLine(ex);
                return null;
           }
        }

        public async Task<bool> DeleteAsync(int userId, int entityId)
        {
            try
            {
                var entity = await GetById(userId, entityId);
                if (entity == null)
                    return true;

                _context.UserProjects.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public IEnumerable<UserProjects?> Filter(Func<UserProjects, bool> func)
        {
            return _context.UserProjects.Where(func).ToList();
        }

        public async Task<UserProjects?> GetById(int userId, int entityId)
        {
            UserProjects? userProjects = await _context.UserProjects
                    .FirstOrDefaultAsync(x => x.User_Id == userId && x.Project_Id == entityId);
            return userProjects;
        }

        public Task<bool> UpdateAsync(UserProjects entity)
        {
            throw new NotImplementedException();
        }
    }
}
