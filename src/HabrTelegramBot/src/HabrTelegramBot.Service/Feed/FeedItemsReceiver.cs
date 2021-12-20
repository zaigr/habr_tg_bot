using HabrTelegramBot.Service.Feed.EventArgs;
using HabrTelegramBot.Service.Feed.Models;
using System.Net.Http.Json;

namespace HabrTelegramBot.Service.Feed;

public class FeedItemsReceiver
{
    private readonly Uri _postsApi;

    private readonly TimeSpan _apiPoolingInterval;

    public FeedItemsReceiver(string postsApi, TimeSpan apiPoolingInterval)
    {
        _apiPoolingInterval = apiPoolingInterval;
        _postsApi = new Uri(postsApi);
    }

    public event EventHandler<FeedItemAddedEventArgs> FeedItemAdded = default!;

    public Task StartObserving(CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
            {
                var httpClient = new HttpClient();
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var fromDate = DateTimeOffset.UtcNow.AddSeconds(-1 * _apiPoolingInterval.TotalSeconds);

                        // TODO: send fromDate to posts API
                        var posts = await httpClient.GetFromJsonAsync<List<FeedItem>>(_postsApi, cancellationToken);

                        NotifyOnNewPostsAdded(posts);
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

    private void NotifyOnNewPostsAdded(IEnumerable<FeedItem>? feedItems)
    {
        if (feedItems is not null)
        {
            foreach (var item in feedItems.Skip(Random.Shared.Next(10, 19))) // TODO: remove random skip (test purpose)
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
}
