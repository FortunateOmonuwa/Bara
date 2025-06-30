using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScriptModule.Models;
using UserModule.Models;

namespace Infrastructure.Configs
{
    internal class ScriptModelBuilderConfig : IEntityTypeConfiguration<Script>
    {
        public void Configure(EntityTypeBuilder<Script> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(s => s.Title);
            builder.HasIndex(s => s.Genre);
            //builder.HasIndex(s => s.WriterId);
            builder.Property(s => s.Title)
                .HasMaxLength(200);
            builder.Property(s => s.Genre)
                .HasMaxLength(100);
            builder.HasOne<Writer>()
                .WithMany()
                .HasForeignKey("WriterId")
            .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(s => s.Status);
            builder.Property(s => s.Status)
                .HasConversion<string>()
                .HasMaxLength(50);
            builder.Property(s => s.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        }
    }
}
