using HabrTelegramBot.Service;
using HabrTelegramBot.Service.BotApi;
using HabrTelegramBot.Service.Feed;
using HabrTelegramBot.Service.Feed.Services;
using Microsoft.Extensions.Configuration;
using Serilog;


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Console()
    .WriteTo.File(
        path: configuration["Serilog:File:PathFormat"],
        fileSizeLimitBytes: configuration.GetValue<int>("Serilog:File:FileSizeLimitBytes"),
        retainedFileCountLimit: configuration.GetValue<int>("Serilog:File:RetainedFileCountLimit"),
        rollOnFileSizeLimit: true
        )
    .CreateLogger();

try
{
    var crawlerApiPollInterval = TimeSpan.FromSeconds(configuration.GetValue<int>("CrawlerServiceApi:PollIntervalSeconds"));
    var feedMessageReceiver = new FeedItemsReceiver(new CrawlerService(configuration["CrawlerServiceApi:Url"]), crawlerApiPollInterval);

    var feedMessageHandler = new FeedMessageHandler(new BotApiService(configuration["BotApi:BotChadId"], configuration["BotApi:AccessToken"]));
    feedMessageReceiver.FeedItemAdded += async (_, e) => await feedMessageHandler.FeedItemAddedAsyncHandler(e);

    Log.Information("Starting observing message feed.");

    await feedMessageReceiver.StartObserving(CancellationToken.None);
}
catch (Exception e)
{
    Log.Error(e, "Fatal error raised.");
    throw;
}
