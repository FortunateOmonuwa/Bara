﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScriptModule.Enums;
using ScriptModule.Models;

namespace ScriptModule.Config.ModelBuilderConfig
{
    internal class ScriptTransactionModelBuilderConfig : IEntityTypeConfiguration<ScriptTransaction>
    {
        public void Configure(EntityTypeBuilder<ScriptTransaction> builder)
        {
            builder.Property<ScriptDeliveryStatus>("Status")
                .HasConversion<string>()
                .HasMaxLength(50);
        }
    }
}
