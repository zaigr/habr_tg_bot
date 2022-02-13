using Azure.Storage;
using ChatBot.Api;
using ChatBot.Api.Telegram;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(ChatBot.AzFunc.AzureCostMonitor.Startup))]

namespace ChatBot.AzFunc.AzureCostMonitor;

public class Startup : FunctionsStartup
{
    private IConfiguration _configuration;

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;

        services.AddTransient<IBotApiClient, TelegramClient>(
            _ => new TelegramClient(_configuration["BotApi:Telegram:ChatId"], _configuration["BotApi:Telegram:Token"]));

        services.AddTransient(_ => new StorageSharedKeyCredential(_configuration["Storage:Account"], _configuration["Storage:Key"]));
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        builder.ConfigurationBuilder
            .AddEnvironmentVariables();

        _configuration = builder.ConfigurationBuilder.Build();
    }
}