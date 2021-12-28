namespace HabrTelegramBot.Rss.Models;

public class ChannelItem
{
    public ChannelItem(
        string title,
        string link,
        string description,
        DateTimeOffset publicationDate,
        IEnumerable<string> categories,
        string? author = null)
    {
        Title = title;
        Link = link;
        Description = description;
        PublicationDate = publicationDate;
        Categories = categories;
        Author = author;
    }

    public string Title { get; }

    public string Link { get; }

    public string Description { get; }

    public DateTimeOffset PublicationDate { get; }

    public IEnumerable<string> Categories { get; }

    public string? Author { get; }
}