using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Models;

namespace UserModule.configs.ModelBuilderConfig
{
    public class ProducerModelBuilderConfig : IEntityTypeConfiguration<Producer>
    {
        void IEntityTypeConfiguration<Producer>.Configure(EntityTypeBuilder<Producer> builder)
        {
            //builder.HasKey(x => x.Id);
            builder.HasIndex(u => u.IsBlacklisted);
            builder.HasIndex(u => u.IsVerified);
            builder.HasIndex(u => u.IsEmailVerified);
            builder.HasIndex(u => u.IsDeleted);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.Gender)
               .HasConversion<string>();
            builder.Property(u => u.VerificationStatus)
             .HasConversion<string>();
        }
    }
}
