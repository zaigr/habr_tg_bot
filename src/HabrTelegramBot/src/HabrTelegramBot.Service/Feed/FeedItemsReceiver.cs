using HabrTelegramBot.Service.Feed.EventArgs;
using HabrTelegramBot.Service.Feed.Services;
using HabrTelegramBot.Service.Feed.Services.Models;

namespace HabrTelegramBot.Service.Feed;

public class FeedItemsReceiver
{
    private readonly TimeSpan _apiPoolingInterval;

    private readonly CrawlerService _crawlerService;

    public FeedItemsReceiver(CrawlerService crawlerService, TimeSpan apiPoolingInterval)
    {
        _crawlerService = crawlerService;
        _apiPoolingInterval = apiPoolingInterval;
    }

    public event EventHandler<FeedItemAddedEventArgs> FeedItemAdded = default!;

    public Task StartObserving(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var posts = await GetLatestPostsAsync(cancellationToken);

                        NotifyOnNewPostsAdded(posts);

                        await Task.Delay(_apiPoolingInterval, cancellationToken);
                    }
                    catch (Exception e)
                    {
                        // TODO: error handler
                        throw;
                    }
                }

                cancellationToken.ThrowIfCancellationRequested();
            },
            cancellationToken);
    }

    private async Task<IEnumerable<FeedItem>?> GetLatestPostsAsync(CancellationToken ct)
    {
        var thresholdSec = 10;
        var fromDate = DateTimeOffset.UtcNow.AddSeconds(-1 * (_apiPoolingInterval.TotalSeconds + thresholdSec));

        return await _crawlerService.GetFeedItemsAsync(fromDate, ct);
    }

    private void NotifyOnNewPostsAdded(IEnumerable<FeedItem>? feedItems)
    {
        if (feedItems is null)
        {
            return;
        }

        foreach (var item in feedItems)
        {
            FeedItemAdded(this, new FeedItemAddedEventArgs(
                feedName: ".Net", // TODO: get feed name from API
                item.Title,
                item.Description,
                item.Link,
                item.PublicationDate,
                item.Categories)
            );
        }
    }
}
