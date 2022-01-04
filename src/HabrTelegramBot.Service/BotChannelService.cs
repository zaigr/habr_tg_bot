using HabrTelegramBot.Data.Services;
using HabrTelegramBot.Rss;
using HabrTelegramBot.Service.BotApi;

namespace HabrTelegramBot.Service;

public class BotChannelService
{
    private readonly BotApiClient _botApiClient;

    private readonly RssFeedReader _rssReader;

    private readonly ChannelItemsIdentityStore _itemIdentityStore;

    public BotChannelService(BotApiClient botApiClient, RssFeedReader rssReader, ChannelItemsIdentityStore itemIdentityStore)
    {
        _botApiClient = botApiClient;
        _rssReader = rssReader;
        _itemIdentityStore = itemIdentityStore;
    }

    public async Task ForwardChannelItemsToBotAsync(string rssChannelUrl)
    {
        var channel = await _rssReader.ReadChannelAsync(rssChannelUrl);

        foreach (var channelItem in channel.Items.OrderBy(i => i.PublicationDate))
        {
            if (await _itemIdentityStore.ContainsAsync(channelItem.Link))
            {
                continue;
            }

            // var textMessage = _formatter.FormatPost()
            var textMessage = $"{channelItem.Description}";

            await _botApiClient.SendTextMessageAsync(textMessage);

            _itemIdentityStore.Add(channelItem.Link);
        }

        await _itemIdentityStore.SaveAsync();
    }
}
