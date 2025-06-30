using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionModule.Models;

namespace Infrastructure.Configs
{
    internal class EscrowModelBuilderConfig : IEntityTypeConfiguration<Escrow>
    {
        public void Configure(EntityTypeBuilder<Escrow> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(50);
            builder.Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");
            builder.HasIndex(e => e.TransactionId);
            builder.HasOne<Transaction>()
                .WithOne()
                .HasForeignKey<Escrow>("TransactionId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
