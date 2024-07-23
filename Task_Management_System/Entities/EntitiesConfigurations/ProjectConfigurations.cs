using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.API.Entities.EntitiesConfigurations
{
    public class ProjectConfigurations : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).HasColumnType("VARCHAR").HasMaxLength(144).IsRequired();
            builder.Property(x => x.Privacy).HasConversion<string>().IsRequired();


            // Relations
            builder.HasOne(x => x.Owner)
                    .WithMany(x => x.OwnedProjects)
                    .HasForeignKey(x => x.Owner_Id)
                    .OnDelete(DeleteBehavior.Cascade);

           
        }
    }
}
