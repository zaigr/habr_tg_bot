using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TelegramBot.AzFunc.RssReader.Core;

namespace TelegramBot.AzFunc.RssReader;

public class PollRssFeedsFunction
{
    private readonly BotChannelService _channelService;

    private readonly IConfiguration _configuration;

    public PollRssFeedsFunction(BotChannelService channelService, IConfiguration configuration)
    {
        _channelService = channelService;
        _configuration = configuration;
    }

    [FunctionName("PollRssFeedsFunction")]
    public async Task Run([TimerTrigger("%FunctionSchedule%", RunOnStartup = true)] TimerInfo timer, ILogger log) 
    {
        log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        try
        {
            var rssLinks = _configuration.GetSection("Rss:Urls").Get<List<string>>();
            foreach (var rssLink in rssLinks)
            {
                await _channelService.ForwardChannelItemsToBotAsync(rssLink);
            }
        }
        catch (Exception e)
        {
            log.LogError(e, "Error occurred during function run.");

            throw;
        }
    }
}
