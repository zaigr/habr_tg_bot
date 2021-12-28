using Newtonsoft.Json;

namespace HabrTelegramBot.Service.Feed.Services.Models;

[JsonObject]
public class FeedItem
{
    [JsonRequired]
    [JsonProperty(PropertyName = "title")]
    public string Title { get; init; } = string.Empty;

    [JsonRequired]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; init; } = string.Empty;

    [JsonRequired]
    [JsonProperty(PropertyName = "link")]
    public string Link { get; init; } = string.Empty;

    [JsonRequired]
    [JsonProperty(PropertyName = "publication_date")]
    public DateTimeOffset PublicationDate { get; init; }

    [JsonRequired]
    [JsonProperty(PropertyName = "categories")]
    public string[] Categories { get; init; } = { };
}
