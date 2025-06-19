using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp_Landing.Services;

namespace WebApp_Landing.Pages;

public class IndexModel : PageModel
{
    private readonly ITelegramService _telegramService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITelegramService telegramService,
            ILogger<IndexModel> logger)
    {
        _telegramService = telegramService;
        _logger = logger;;
    }

    [BindProperty]
    public string Username { get; set; }

    public void OnGet()
    {
    }
    
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPostSubscribeAsync()
    {
        try
            {
                bool success = await _telegramService.SendWelcomeMessageAsync(Username);
                
                if (success)
                {
                    TempData["Message"] = "Сообщение отправлено в Telegram!";
                }
                else
                {
                    TempData["Error"] = "Не удалось отправить сообщение. Проверьте username.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке в Telegram"); 
                TempData["Error"] = "Произошла ошибка сервера";
            }

        return RedirectToPage();
    }
}
