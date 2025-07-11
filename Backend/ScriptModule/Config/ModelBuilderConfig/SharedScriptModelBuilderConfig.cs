using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScriptModule.Models;
namespace ScriptModule.Config.ModelBuilderConfig
{
    internal class SharedScriptModelBuilderConfig : IEntityTypeConfiguration<SharedScript>
    {
        public void Configure(EntityTypeBuilder<SharedScript> builder)
        {

            //builder.HasKey(x => x.ScriptId);
            //builder.HasIndex(x => x.ScriptId);
            //uilder.HasIndex(X => X.WriterId);
            //builder.HasIndex(X => X.ProducerId);
            //builder.HasOne<Script>()
            //    .WithMany()
            //    .HasForeignKey(x => x.ScriptId)
            //    .OnDelete(DeleteBehavior.Cascade);
            //builder.HasOne("Writer")
            //    .WithMany("SharedScripts")
            //    .HasForeignKey("WriterId")
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne("Producer")
            //    .WithMany("SharedScripts")
            //    .HasForeignKey("ProducerId")
            //    .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
