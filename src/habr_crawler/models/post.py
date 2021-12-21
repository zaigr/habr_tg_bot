from datetime import datetime

class Post:
    def __init__(self, title: str, link: str, description: str, publication_date: datetime, categories: list[str]) -> None:
        self.title = title
        self.link = link
        self.description = description
        self.publication_date = publication_date
        self.categories = categories

    def to_json(self) -> any:
        return {
            'title': self.title,
            'link': self.link,
            'description': self.description,
            'publication_date': self.publication_date,
            'categories': self.categories
        }

    def create_from(object: any):
        return Post(
            object.title,
            object.link,
            object.description,
            object.publication_date,
            object.categories
        )
