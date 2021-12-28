using HabrTelegramBot.Rss;
using HabrTelegramBot.Service.BotApi;

namespace HabrTelegramBot.Service;

public class BotChannelService
{
    private readonly BotApiClient _botApiClient;

    private readonly RssFeedReader _rssReader;

    public BotChannelService(BotApiClient botApiClient, RssFeedReader rssReader)
    {
        _botApiClient = botApiClient;
        _rssReader = rssReader;
    }

    public async Task ForwardChannelItemsToBotAsync(string rssChannelUrl)
    {
        var channel = await _rssReader.ReadChannelAsync(rssChannelUrl);

        foreach (var channelItem in channel.Items.OrderBy(i => i.PublicationDate))
        {
            // if feedItemsStore.Contains(item)  => continue

            // var textMessage = _formatter.FormatPost()
            var textMessage = $"{channelItem.Description}";

            await _botApiClient.SendTextMessageAsync(textMessage);
        }
    }
}
