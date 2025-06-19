using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_Landing.Pages;

public class Error404Model : PageModel
{
    public void OnGet()
    {
        Response.StatusCode = 404; // Устанавливаем HTTP-статус 404
    }
}
