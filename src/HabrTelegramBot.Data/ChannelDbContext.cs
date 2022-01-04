using HabrTelegramBot.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HabrTelegramBot.Data;

public class ChannelDbContext : DbContext
{
    public ChannelDbContext(DbContextOptions options)
        : base(options)
    {
    }

    internal DbSet<ChannelItemIdentity> ChannelItems { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChannelDbContext).Assembly);
    }
}
