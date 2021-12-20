using HabrTelegramBot.Service.Feed.EventArgs;

namespace HabrTelegramBot.Service;

internal class FeedMessageHandler
{
    public Task FeedItemAddedHandler(FeedItemAddedEventArgs args)
    {
        return Task.CompletedTask;
    }
}
