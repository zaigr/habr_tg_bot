using HabrTelegramBot.Service;
using HabrTelegramBot.Service.Feed;

var botApiKey = "5024272889:AAESgCDdASyzYBpVzLpRB6tsEpdaM4b7do4";
var botUsername = "348317615";
var crawlerApi = "http://127.0.0.1:5000/posts";

var feedMessageReceiver = new FeedItemsReceiver(crawlerApi, TimeSpan.FromMinutes(1));
var feedMessageHandler = new FeedMessageHandler();

feedMessageReceiver.FeedItemAdded += async (_, e) => await feedMessageHandler.FeedItemAddedHandler(e);

Console.WriteLine("Press Ctrl+C to exit.");
