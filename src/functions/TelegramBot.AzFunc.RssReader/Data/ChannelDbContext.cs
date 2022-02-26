using Microsoft.EntityFrameworkCore;
using TelegramBot.AzFunc.RssReader.Data.Entities;

namespace TelegramBot.AzFunc.RssReader.Data;

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
