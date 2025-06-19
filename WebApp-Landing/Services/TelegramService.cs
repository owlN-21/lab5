using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace WebApp_Landing.Services;

public class TelegramService : ITelegramService
{
    private readonly List<string> _randomMessages = new()
    {
        "Привет, {0}! У тебя всё получится! 💪",
        "{0}, ты справишься! Я верю в тебя! 🚀",
        "Не сдавайся, {0}! Каждая проблема — это шаг к успеху. 🌟",
        "{0}, помни: даже ошибки — это часть пути! 🛤️",
        "Ты крут, {0}! Продолжай в том же духе! 🔥"
    };
    private readonly HttpClient _httpClient;
    private readonly string _botToken;
    private readonly string _chatId;

    public TelegramService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _botToken = configuration["Telegram:BotToken"];
        _chatId = configuration["Telegram:ChatId"];
    }

    private string GetRandomMessage(string username)
    {
        Random rnd = new Random();
        int index = rnd.Next(_randomMessages.Count);
        return string.Format(_randomMessages[index], username); // Подставляем имя пользователя
    }

    public async Task<bool> SendWelcomeMessageAsync(string username)
    {
        if (string.IsNullOrEmpty(_botToken) || !username.StartsWith("@"))
            return false;

        string cleanUsername = username[1..]; 

        // Получаем chat_id пользователя через getUpdates
        var updatesUrl = $"https://api.telegram.org/bot{_botToken}/getUpdates";
        var updatesResponse = await _httpClient.GetFromJsonAsync<TelegramUpdates>(updatesUrl);

        // Находим пользователя среди чатов
        var userChat = updatesResponse.Result
            .FirstOrDefault(u => u.Message?.From?.Username == cleanUsername);

        if (userChat == null)
            return false;

        string randomText = GetRandomMessage(username);
        // Отправляем сообщение
        var sendMessageUrl = $"https://api.telegram.org/bot{_botToken}/sendMessage";
        var response = await _httpClient.PostAsJsonAsync(sendMessageUrl, new
        {
            chat_id = userChat.Message.Chat.Id,
            text = randomText
        });

        return response.IsSuccessStatusCode;
    }
}