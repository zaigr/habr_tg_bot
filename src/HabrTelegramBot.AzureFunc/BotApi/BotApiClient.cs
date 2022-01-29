using System.Threading.Tasks;
using Telegram.Bot;

namespace HabrTelegramBot.AzureFunc.BotApi;

public class BotApiClient
{
    private readonly string _chatId;

    private readonly TelegramBotClient _botClient;

    public BotApiClient(string chatId, string apiAccessToken)
    {
        _chatId = chatId;
        _botClient = new TelegramBotClient(apiAccessToken);
    }

    public async Task SendTextMessageAsync(string text)
    {
        await _botClient.SendTextMessageAsync(_chatId, text);
    }
}