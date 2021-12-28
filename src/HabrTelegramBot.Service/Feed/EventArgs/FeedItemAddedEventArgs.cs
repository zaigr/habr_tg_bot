namespace HabrTelegramBot.Service.Feed.EventArgs;

public class FeedItemAddedEventArgs : System.EventArgs
{
    public FeedItemAddedEventArgs(
        string feedName,
        string title,
        string description,
        string link,
        DateTimeOffset publicationDate,
        string[] categories)
    {
        FeedName = feedName;
        Title = title;
        Description = description;
        Link = link;
        PublicationDate = publicationDate;
        Categories = categories;
    }

    public string FeedName { get; }

    public string Title { get; }

    public string Description { get; }

    public string Link { get; }

    public DateTimeOffset PublicationDate { get; }

    public string[] Categories { get; }
}
