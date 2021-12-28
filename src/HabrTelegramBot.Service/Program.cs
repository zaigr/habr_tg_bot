using HabrTelegramBot.Rss;
using HabrTelegramBot.Service;
using HabrTelegramBot.Service.BotApi;
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

var botApiClient = new BotApiClient(configuration["BotApi:BotChadId"], configuration["BotApi:AccessToken"]);
var rssReader = new RssFeedReader(new HttpClient());
var botChannelService = new BotChannelService(botApiClient, rssReader);

var crawlerApiPollInterval = TimeSpan.FromSeconds(configuration.GetValue<int>("CrawlerServiceApi:PollIntervalSeconds"));
var rssLinks = new[] { "https://habr.com/ru/rss/hub/net/all/?fl=ru" };

Log.Information("Starting observing message feed.");

while (true)
{
    foreach (var rssLink in rssLinks)
    {
        try
        {
            await botChannelService.ForwardChannelItemsToBotAsync(rssLink);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception raised during RSS feed forwarding.");
        }

        await Task.Delay(crawlerApiPollInterval);
    }
}
