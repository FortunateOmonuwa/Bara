using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Models;

namespace UserModule.configs.ModelBuilderConfig
{
    public class RoleModelBuilderConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.HasData(new Role { Id = 1, Name = "Admin" }, new Role { Id = 2, Name = "Writer" }, new Role { Id = 3, Name = "Producer" });
        }
    }
}
