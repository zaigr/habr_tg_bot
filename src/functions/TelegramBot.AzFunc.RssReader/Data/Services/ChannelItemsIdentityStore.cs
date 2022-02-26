using System.Threading.Tasks;
using TelegramBot.AzFunc.RssReader.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace TelegramBot.AzFunc.RssReader.Data.Services;

public class ChannelItemsIdentityStore
{
    private readonly ChannelDbContext _dbContext;

    public ChannelItemsIdentityStore(ChannelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ContainsAsync(string itemLink)
    {
        return await _dbContext.ChannelItems.AnyAsync(e => e.ItemLink == itemLink);
    }

    public void Add(string itemLink)
    {
        _dbContext.ChannelItems.Add(new ChannelItemIdentity(itemLink));
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
