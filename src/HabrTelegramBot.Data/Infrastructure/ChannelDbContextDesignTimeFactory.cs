using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HabrTelegramBot.Data.Infrastructure;

public class ChannelDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ChannelDbContext>
{
    public ChannelDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ChannelDbContext>();
        optionsBuilder.UseSqlServer("");

        return new ChannelDbContext(optionsBuilder.Options);
    }
}
