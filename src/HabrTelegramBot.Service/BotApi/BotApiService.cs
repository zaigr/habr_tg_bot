using Telegram.Bot;

namespace HabrTelegramBot.Service.BotApi;

public class BotApiService
{
    private readonly string _chatId;

    private readonly TelegramBotClient _botClient;

    public BotApiService(string chatId, string apiAccessToken)
    {
        _chatId = chatId;
        _botClient = new TelegramBotClient(apiAccessToken);
    }

    public async Task SendTextMessageAsync(string text)
    {
        await _botClient.SendTextMessageAsync(_chatId, text);
    }
}