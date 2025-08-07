using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Models;

namespace UserModule.configs.ModelBuilderConfig
{
    public class UserModelBuilderConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(x => x.Id);
            builder.HasIndex(x => x.Email);
            builder.HasIndex(x => x.IsBlacklisted);
            builder.HasIndex(x => x.Gender);
        }
    }
}
