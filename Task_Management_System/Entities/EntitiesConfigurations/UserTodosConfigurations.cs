using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.API.Entities.EntitiesConfigurations
{
    public class UserTodosConfigurations : IEntityTypeConfiguration<UserTodos>
    {
        public void Configure(EntityTypeBuilder<UserTodos> builder)
        {
            builder.ToTable("UserTodos");
            builder.HasKey(x => new { x.User_Id, x.Todo_Id });


            // Relations
            builder.HasOne(x => x.User)
                .WithMany(x => x.AssignedTodos)
                .HasForeignKey(x => x.User_Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Todo)
                    .WithMany(x => x.AssignedUsers)
                    .HasForeignKey(x => x.Todo_Id)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
