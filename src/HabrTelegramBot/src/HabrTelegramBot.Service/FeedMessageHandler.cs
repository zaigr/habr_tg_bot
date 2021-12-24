using HabrTelegramBot.Service.BotApi;
using HabrTelegramBot.Service.Feed.EventArgs;
using Serilog;

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
            Log.Error(e, "Exception raised during feed message processing");
        }
    }

    private string GetFormattedMessageText(FeedItemAddedEventArgs args)
    {
        return args.Link;
    }
}
