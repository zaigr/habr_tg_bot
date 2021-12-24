using HabrTelegramBot.Service.BotApi;
using HabrTelegramBot.Service.Feed.EventArgs;

namespace HabrTelegramBot.Service;

internal class FeedMessageHandler
{
    private readonly BotApiService _botApiService;

    public FeedMessageHandler(BotApiService botApiService)
    {
        _botApiService = botApiService;
    }

    public async Task FeedItemAddedAsyncHandler(FeedItemAddedEventArgs args)
    {
        try
        {
            var messageText = GetFormattedMessageText(args);

            await _botApiService.SendTextMessageAsync(messageText);
        }
        catch (Exception e)
        {
            // log exception here
            throw;
        }
    }

    private string GetFormattedMessageText(FeedItemAddedEventArgs args)
    {
        return args.Link;
    }
}
