using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Models;

namespace Infrastructure.Configs
{
    internal class ServiceModelConfigurationBuilder : IEntityTypeConfiguration<Services>
    {
        public void Configure(EntityTypeBuilder<Services> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(s => s.IPDealType)
                .HasConversion<string>()
                .HasMaxLength(50);
            builder.Property(s => s.PaymentType)
                .HasConversion<string>()
                .HasMaxLength(50);
            builder.HasIndex(s => s.WriterId);
            builder.HasOne<Writer>()
                .WithMany(w => w.Services)
                .HasForeignKey(s => s.WriterId)
                .OnDelete(DeleteBehavior.Cascade);
            //builder.HasQueryFilter(s => !s.ScriptWriter.IsDeleted);

        }
    }
}
