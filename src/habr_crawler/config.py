import json

RSS_FEEDS: list[str]

_configuration_file = "configuration.json"

with open(_configuration_file, 'r') as config:
    json_data = json.load(config)

    RSS_FEEDS = json_data["rss_feeds"]
