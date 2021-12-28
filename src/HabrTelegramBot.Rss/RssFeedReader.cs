using System.Xml;
using System.ServiceModel.Syndication;
using HabrTelegramBot.Models;

namespace HabrTelegramBot.Rss;

public class RssFeedReader
{
    private readonly HttpClient _httpClient;

    public RssFeedReader(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Channel> ReadChannelAsync(string rssUrl)
    {
        var responseStream = await _httpClient.GetStreamAsync(rssUrl);

        var feed = SyndicationFeed.Load(new XmlTextReader(responseStream));

        var channelItems = GetChannelItems(feed.Items);

        return new Channel(feed.Title.Text, feed.Id, feed.Description.Text, channelItems);
    }

    private IList<ChannelItem> GetChannelItems(IEnumerable<SyndicationItem> syndicationItems)
        => syndicationItems
            .Select(i 
                => new ChannelItem(
                    title: i.Title.Text,
                    link: i.Id,
                    description: i.Summary.Text,
                    publicationDate: i.PublishDate,
                    categories: i.Categories.Select(category => category.Name).ToList(),
                    author: i.Authors.FirstOrDefault()?.Name))
            .ToList();
}
