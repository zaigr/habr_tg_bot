using System.IO;
using System.Net.Http;
using TelegramBot.AzFunc.RssReader.Data.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TelegramBot.AzFunc.RssReader;
using TelegramBot.AzFunc.RssReader.Core;
using TelegramBot.AzFunc.RssReader.Data;
using TelegramBot.AzFunc.RssReader.Rss;
using TelegramBot.Client;
using TelegramBot.Client.Telegram;

[assembly: FunctionsStartup(typeof(Startup))]

namespace TelegramBot.AzFunc.RssReader;

public class Startup : FunctionsStartup
{
    private IConfiguration _configuration;

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;

        services.AddTransient<IBotApiClient>(_
            => new TelegramClient(_configuration["BotApi:BotChadId"], _configuration["BotApi:AccessToken"]));

        services.AddTransient(_ => new RssFeedReader(new HttpClient()));

        services.AddDbContext<ChannelDbContext>(
            options => options.UseSqlServer(_configuration.GetConnectionString("ChannelDb")));

        services.AddTransient<ChannelItemsIdentityStore>();

        services.AddTransient<BotChannelService>();
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        FunctionsHostBuilderContext context = builder.GetContext();

        builder.ConfigurationBuilder
            .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
            .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{context.EnvironmentName}.json"), optional: true, reloadOnChange: false)
            .AddEnvironmentVariables();

        _configuration = builder.ConfigurationBuilder.Build();
    }
}
