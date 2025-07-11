using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Models;

namespace UserModule.configs.ModelBuilderConfig
{
    public class ServiceModelConfigurationBuilder : IEntityTypeConfiguration<Services>
    {
        public void Configure(EntityTypeBuilder<Services> builder)
        {
            //builder.HasKey(x => x.Id);
            builder.Property(s => s.IPDealType)
                .HasConversion<string>()
                .HasMaxLength(50);
            builder.Property(s => s.PaymentType)
                .HasConversion<string>()
                .HasMaxLength(50);
            //builder.HasOne<Writer>()
            //    .WithMany(w => w.Services)
            //    .HasForeignKey(s => s.ScriptWriterId)
            //    .OnDelete(DeleteBehavior.Cascade);
            //builder.HasIndex(s => s.WriterId);
            //builder.HasQueryFilter(s => !s.ScriptWriter.IsDeleted);

        }
    }
}
