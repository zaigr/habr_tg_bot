using HabrTelegramBot.Service;
using HabrTelegramBot.Service.Feed;
using HabrTelegramBot.Service.Feed.Services;
using Microsoft.Extensions.Configuration;


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var crawlerApiPollInterval = TimeSpan.FromSeconds(int.Parse(configuration["CrawlerServiceApi:PollIntervalSeconds"]));
var feedMessageReceiver = new FeedItemsReceiver(new CrawlerService(configuration["CrawlerServiceApi:Url"]), crawlerApiPollInterval);

var feedMessageHandler = new FeedMessageHandler();
feedMessageReceiver.FeedItemAdded += async (_, e) => await feedMessageHandler.FeedItemAddedHandler(e);

await feedMessageReceiver.StartObserving(CancellationToken.None);

Console.WriteLine("Press Ctrl+C to exit.");
