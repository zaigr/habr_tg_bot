using HabrTelegramBot.Service.Feed.Services.Models;
using System.Net.Http.Json;

namespace HabrTelegramBot.Service.Feed.Services;

public class CrawlerService
{
    private readonly Uri _baseUri;

    private readonly HttpClient _httpClient;

    public CrawlerService(string baseUri)
    {
        _baseUri = new Uri(baseUri);
        _httpClient = new HttpClient();
    }

    public async Task<IEnumerable<FeedItem>?> GetFeedItemsAsync(DateTimeOffset fromDate, CancellationToken ct = default)
    {
        return await _httpClient.GetFromJsonAsync<List<FeedItem>>(new Uri(_baseUri, $"posts?from={Uri.EscapeDataString(fromDate.ToString())}"), ct);
    }
}

