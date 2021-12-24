using HabrTelegramBot.Service;
using HabrTelegramBot.Service.BotApi;
using HabrTelegramBot.Service.Feed;
using HabrTelegramBot.Service.Feed.Services;
using Microsoft.Extensions.Configuration;


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var crawlerApiPollInterval = TimeSpan.FromSeconds(int.Parse(configuration["CrawlerServiceApi:PollIntervalSeconds"]));
var feedMessageReceiver = new FeedItemsReceiver(new CrawlerService(configuration["CrawlerServiceApi:Url"]), crawlerApiPollInterval);

var feedMessageHandler = new FeedMessageHandler(new BotApiService(configuration["BotApi:BotChadId"], configuration["BotApi:AccessToken"]));
feedMessageReceiver.FeedItemAdded += async (_, e) => await feedMessageHandler.FeedItemAddedAsyncHandler(e);

await feedMessageReceiver.StartObserving(CancellationToken.None);

Console.WriteLine("Press Ctrl+C to exit.");
