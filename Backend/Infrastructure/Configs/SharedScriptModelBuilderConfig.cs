using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScriptModule.Models;
using UserModule.Models;

namespace Infrastructure.Configs
{
    internal class SharedScriptModelBuilderConfig : IEntityTypeConfiguration<SharedScript>
    {
        public void Configure(EntityTypeBuilder<SharedScript> builder)
        {
            builder.HasKey(x => x.ScriptId);
            builder.HasIndex(x => x.ScriptId);
            builder.HasIndex(X => X.WriterId);
            builder.HasIndex(X => X.ProducerId);
            //builder.HasOne<Script>()
            //    .WithMany()
            //    .HasForeignKey(x => x.ScriptId)
            //    .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<Writer>()
                .WithMany(w => w.SharedScripts)
                .HasForeignKey(x => x.WriterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Producer>()
                .WithMany(p => p.SharedScripts)
                .HasForeignKey(x => x.ProducerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
