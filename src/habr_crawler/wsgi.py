import datetime
from models.post import Post
from crawler_core.crawler import Crawler, PostItem
from flask import Flask, jsonify

app = Flask(__name__)

@app.route("/posts", methods = ['GET'])
def get_posts():
    """returns: latest posts from observed feeds"""

    # config = Config()
    # crawler = Crawler()

    # posts = crawler.get_latest_posts()

    #   parser = Parser()
    #   rss = parser.get_rss_feeds()
    #   latest_posts = [deserialize_post(post) for post in rss.posts]
    crawler = Crawler(["https://habr.com/ru/rss/hub/net/all/?fl=ru"])
    posts = [Post(
        title=item.title,
        link=item.link,
        description=item.description,
        publication_date=datetime.date(2000, 3, 3),
        categories=["a", "b"],
        ) for item in crawler.get_latest_posts()]

    return jsonify([post.to_json() for post in posts])
