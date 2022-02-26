namespace TelegramBot.Client;

public interface IBotApiClient
{
    Task SendTextMessageAsync(string message);
}