using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp_Landing.Pages;

public class TosModel : PageModel
{
    private readonly ILogger<TosModel> _logger;

    public TosModel(ILogger<TosModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}

