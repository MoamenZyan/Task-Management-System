using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.API.Entities.EntitiesConfigurations
{
    public class UserProjectsConfigurations : IEntityTypeConfiguration<UserProjects>
    {
        public void Configure(EntityTypeBuilder<UserProjects> builder)
        {
            builder.ToTable("UserProjects");
            builder.HasKey(x => new {x.User_Id, x.Project_Id});

            builder.Property(x => x.Role).HasConversion<string>().IsRequired();

            // Relations
            builder.HasOne(x => x.User)
                .WithMany(x => x.JoinedProjects)
                .HasForeignKey(x => x.User_Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Project)
                    .WithMany(x => x.Users)
                    .HasForeignKey(x => x.Project_Id)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
