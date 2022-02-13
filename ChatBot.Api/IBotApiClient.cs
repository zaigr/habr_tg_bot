namespace ChatBot.Api;

public interface IBotApiClient
{
    Task SendTextMessageAsync(string message);
}