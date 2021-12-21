import datetime
from .rss_feed import RssFeed, FeedItem


class PostItem:
    def __init__(self, title: str, description: str, link: str, publication_date: datetime.datetime, categories: list[str]) -> None:
        self.title = title
        self.link = link
        self.description = description
        self.publication_date = publication_date
        self.categories = categories

    def create_from(object: any):
        return PostItem(
            object.title,
            object.link,
            object.description,
            object.publication_date,
            object.categories
        )


class Crawler:
    def __init__(self, sources: list[str]) -> None:
        self.sources = sources

    def get_latest_posts(self, from_date: datetime.datetime = None) -> list[PostItem]:
        all_feeds: list[FeedItem] = []
        for rss_link in self.sources:
            feed = RssFeed(rss_link)

            feed.load()
            all_feeds += feed.get_items()

        latest_feeds = filter(lambda i: i.publication_date >= from_date, all_feeds) if from_date is not None else all_feeds
        posts = [PostItem.create_from(item) for item in latest_feeds]

        return sorted(posts, key=lambda post: post.publication_date, reverse=True)
