using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionModule.Models;

namespace TransactionModule.Config.ModelBuilderConfig
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
            builder.Property(t => t.Currency)
         .HasConversion<string>();
            //builder.HasOne("Producer")
            //    .WithMany()
            //    .HasForeignKey("ProducerId")
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne("Producer")
            //    .WithMany()
            //    .HasForeignKey("WriterId")
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne("Script")
            //    .WithMany()
            //    .HasForeignKey("ScriptId")
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(t => t.TransactionType);
            builder.HasIndex(t => t.Status);
            builder.Property(x => x.Amount)
                .IsRequired();
            //builder.HasIndex(t => t.ProducerId);
            //builder.HasIndex(t => t.WriterId);
            //builder.HasIndex(t => t.ScriptId);
            //builder.HasIndex(t => t.ReferenceId);
        }
    }
}
