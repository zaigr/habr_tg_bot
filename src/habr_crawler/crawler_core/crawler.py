import requests
import datetime
import xml.etree.ElementTree as ET


class PostItem:
    def __init__(self, title, description, link) -> None:
        self.title = title
        self.description = description
        self.link = link


class Crawler:
    def __init__(self, sources: list[str]):
        #self._parser = Parser()
        self.sources = sources

    def get_latest_posts(self) -> list[PostItem]:
        rss_feed = requests.get(self.sources[0])

        rss_root = ET.fromstring(rss_feed.text)
        channel_node = rss_root.find('channel')

        posts = []
        for item in channel_node.findall('item'):
            posts.append(
                PostItem(
                    title=item.find('title').text,
                    link=item.find('guid').text,
                    description=item.find('description').text,
                    #publication_date=datetime.date(2019, 3, 3),
                    #categories=["a"]
                )
            )

        return posts
