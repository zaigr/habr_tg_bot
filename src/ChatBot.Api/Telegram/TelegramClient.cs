using Telegram.Bot;

namespace ChatBot.Api.Telegram;

public class TelegramClient : IBotApiClient
{
    private readonly string _chatId;

    private readonly TelegramBotClient _botClient;

    public TelegramClient(string chatId, string accessToken)
    {
        _chatId = chatId;

        _botClient = new TelegramBotClient(accessToken);
    }

    public async Task SendTextMessageAsync(string message)
    {
        await _botClient.SendTextMessageAsync(_chatId, message);
    }
}