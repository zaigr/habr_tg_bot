﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HabrTelegramBot.Data.Entities.Configurations;

internal class ChannelItemIdentityEntityConfiguration : IEntityTypeConfiguration<ChannelItemIdentity>
{
    public void Configure(EntityTypeBuilder<ChannelItemIdentity> builder)
    {
        builder
            .HasKey(e => e.ItemLink);
    }
}