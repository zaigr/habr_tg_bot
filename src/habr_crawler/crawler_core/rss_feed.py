import requests
import datetime
import xml.etree.ElementTree as ET
from utils.format import to_datetime


class FeedItem:
    def __init__(self, title: str, link: str, description: str, publication_date: datetime.datetime, creator: str, categories: list[str]) -> None:
        self.title = title
        self.link = link
        self.description = description
        self.publication_date = publication_date
        self.creator = creator
        self.categories = categories

    def from_xml(xml_node: ET.Element):
        return FeedItem(
            title=xml_node.find('title').text,
            link=xml_node.find('guid').text,
            description=xml_node.find('description').text,
            publication_date=to_datetime(xml_node.find('pubDate').text),
            creator="", #xml_node.find('dc:creator').text,
            categories=[node.text for node in xml_node.findall('category')]
        )


class RssFeed:
    def __init__(self, rss_feed_link: str) -> None:
        self.rss_feed_link = rss_feed_link
        self.title: str = None
        self.link: str = None
        self.description: str = None
        self.language: str = None
        self.publication_date: datetime.datetime = None
        self.__items: list[FeedItem] = []

    def load(self) -> None:
        xml_response = requests.get(self.rss_feed_link)

        root = ET.fromstring(xml_response.text)
        channel_node = root.find('channel')

        self.title = channel_node.find('title').text
        self.link = channel_node.find('link').text
        self.description = channel_node.find('description').text
        self.language = channel_node.find('language').text
        self.publication_date = to_datetime(channel_node.find('pubDate').text)

        for item_node in channel_node.findall('item'):
            self.__items.append(FeedItem.from_xml(item_node))

    def get_items(self) -> list[FeedItem]:
        return list(self.__items)

