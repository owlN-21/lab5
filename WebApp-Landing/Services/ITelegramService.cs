
namespace WebApp_Landing.Services;

public interface ITelegramService
{
     Task<bool> SendWelcomeMessageAsync(string username); 
}