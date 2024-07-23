using Microsoft.EntityFrameworkCore;
using Project.API.Entities;
using Project.API.Entities.EntitiesConfigurations;

namespace Task_Management_System.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Project.API.Entities.Project> Projects { get; set; }
        public DbSet<UserProjects> UserProjects { get; set; }
        public DbSet<UserTodos> UserTodos { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfigurations());
            modelBuilder.ApplyConfiguration(new TodoConfigurations());
            modelBuilder.ApplyConfiguration(new ProjectConfigurations());
            modelBuilder.ApplyConfiguration(new UserTodosConfigurations());
            modelBuilder.ApplyConfiguration(new UserProjectsConfigurations());
            base.OnModelCreating(modelBuilder);
        }
    }
}
