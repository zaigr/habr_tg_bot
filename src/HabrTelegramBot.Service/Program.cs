using HabrTelegramBot.Data;
using HabrTelegramBot.Data.Services;
using HabrTelegramBot.Rss;
using HabrTelegramBot.Service;
using HabrTelegramBot.Service.BotApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;


var configuration = BuildConfiguration();
Log.Logger = ConfigureLogger(configuration);

var botApiClient = new BotApiClient(configuration["BotApi:BotChadId"], configuration["BotApi:AccessToken"]);
var rssReader = new RssFeedReader(new HttpClient());
var botChannelService = new BotChannelService(botApiClient, rssReader, new ChannelItemsIdentityStore(ConfigureDbContext(configuration)));

var rssPollInterval = TimeSpan.FromSeconds(configuration.GetValue<int>("Rss:PollIntervalSeconds"));
var rssLinks = configuration.GetSection("Rss:Urls").Get<List<string>>();

Log.Information($"Start observing RSS feeds with interval '{rssPollInterval}'");

while (true)
{
    foreach (var rssLink in rssLinks)
    {
        Log.Information($"Process RSS feed '{rssLink}'");

        try
        {
            await botChannelService.ForwardChannelItemsToBotAsync(rssLink);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception raised during RSS feed forwarding.");
        }

        await Task.Delay(rssPollInterval);
    }
}

static IConfiguration BuildConfiguration()
{
    return new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();
}

static ILogger ConfigureLogger(IConfiguration configuration)
{
    return new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .WriteTo.Console()
        .WriteTo.File(
            path: configuration["Serilog:File:PathFormat"],
            fileSizeLimitBytes: configuration.GetValue<int>("Serilog:File:FileSizeLimitBytes"),
            retainedFileCountLimit: configuration.GetValue<int>("Serilog:File:RetainedFileCountLimit"),
            rollOnFileSizeLimit: true
        )
        .CreateLogger();
}

static ChannelDbContext ConfigureDbContext(IConfiguration configuration)
{
    var optionsBuilder = new DbContextOptionsBuilder<ChannelDbContext>();
    optionsBuilder.UseSqlServer(configuration.GetConnectionString("ChannelDb"));

    return new ChannelDbContext(optionsBuilder.Options);
}
