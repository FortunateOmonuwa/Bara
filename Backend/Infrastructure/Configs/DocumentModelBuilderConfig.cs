using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserModule.Models;

namespace Infrastructure.Configs
{
    internal class DocumentModelBuilderConfig : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.DocumentType)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}
