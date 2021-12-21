import datetime
import dateutil.parser

def to_datetime(datetime_str: str) -> datetime.datetime:
    if datetime_str is None:
        return None

    return dateutil.parser.parse(datetime_str)
