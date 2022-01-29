using System.IO;
using System.Net.Http;
using HabrTelegramBot.AzureFunc.BotApi;
using HabrTelegramBot.AzureFunc.Core;
using HabrTelegramBot.Data;
using HabrTelegramBot.Data.Services;
using HabrTelegramBot.Rss;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(HabrTelegramBot.AzureFunc.Startup))]

namespace HabrTelegramBot.AzureFunc
{
    public class Startup : FunctionsStartup
    {
        private IConfiguration _configuration;

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            services.AddTransient(_
                => new BotApiClient(_configuration["BotApi:BotChadId"], _configuration["BotApi:AccessToken"]));

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
}
