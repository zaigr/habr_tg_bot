import config
from flask import Flask, request, jsonify
from models.post import Post
from crawler_core.crawler import Crawler
from utils.format import to_datetime

app = Flask(__name__)

@app.route("/v1/posts", methods = ['GET'])
def get_posts():
    """returns: latest posts from observed feeds"""
    from_date = to_datetime(request.args.get('from'))

    crawler = Crawler(config.RSS_FEEDS)

    posts: list[Post] = []
    for item in crawler.get_latest_posts(from_date):
        posts.append(Post.create_from(item))

    return jsonify([post.to_json() for post in posts])
