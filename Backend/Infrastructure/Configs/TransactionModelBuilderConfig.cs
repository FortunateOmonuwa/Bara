using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScriptModule.Models;
using TransactionModule.Models;
using UserModule.Models;

namespace Infrastructure.Configs
{
    internal class TransactionModelBuilderConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(t => t.TransactionType)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.HasKey(t => t.Id);

            builder.HasOne<Producer>()
                .WithMany()
                .HasForeignKey("ProducerId")
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Writer>()
                .WithMany()
                .HasForeignKey("WriterId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Script>()
                .WithMany()
                .HasForeignKey(t => t.ScriptId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(t => t.ProducerId);
            builder.HasIndex(t => t.WriterId);
            builder.HasIndex(t => t.ScriptId);
            builder.HasIndex(t => t.Status);
            builder.HasIndex(t => t.TransactionType);
            builder.HasIndex(t => t.ReferenceId);
            builder.Property(x => x.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
