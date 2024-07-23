using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.API.Entities.EntitiesConfigurations
{
    public class TodoConfigurations : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.ToTable("Todos");
            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).HasColumnType("VARCHAR").HasMaxLength(144).IsRequired();
            builder.Property(x => x.Deadline).HasColumnType("DATETIME").IsRequired();
            builder.Property(x => x.Status).HasConversion<string>();


            // Relations
            builder.HasOne(x => x.Owner)
                    .WithMany(x => x.OwnedTodos)
                    .HasForeignKey(x => x.Owner_Id)
                    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Project)
                    .WithMany(x => x.Todos)
                    .HasForeignKey(x => x.Project_Id)
                    .OnDelete(DeleteBehavior.Cascade);
                    
        }
    }
}
