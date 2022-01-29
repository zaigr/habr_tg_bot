using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HabrTelegramBot.AzureFunc.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HabrTelegramBot.AzureFunc
{
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
        public async Task Run([TimerTrigger("%FunctionSchedule%")] TimerInfo timer, ILogger log) 
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
}
