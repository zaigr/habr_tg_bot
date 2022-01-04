namespace HabrTelegramBot.Rss.Models;

public class Channel
{
    public Channel(string title, string link, string description, IEnumerable<ChannelItem> items)
    {
        Title = title;
        Link = link;
        Description = description;
        Items = items;
    }

    public string Title { get; }

    public string Link { get; }

    public string Description { get; }

    public IEnumerable<ChannelItem> Items { get; }
}
