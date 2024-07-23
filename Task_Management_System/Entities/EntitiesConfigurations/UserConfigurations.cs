using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Project.API.Entities.EntitiesConfigurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Fname).HasColumnType("VARCHAR").HasMaxLength(144).IsRequired();
            builder.Property(x => x.Lname).HasColumnType("VARCHAR").HasMaxLength(144).IsRequired();
            builder.Property(x => x.Email).HasColumnType("VARCHAR").HasMaxLength(255).IsRequired();
            builder.Property(x => x.Password).HasColumnType("VARCHAR").HasMaxLength(255).IsRequired();
            builder.Property(x => x.Phone).HasColumnType("VARCHAR").HasMaxLength(15).IsRequired();
        }
    }
}
