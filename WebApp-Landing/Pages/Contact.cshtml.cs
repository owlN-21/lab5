using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CsvHelper;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace WebApp_Landing.Pages;

[IgnoreAntiforgeryToken]
public class ContactModel : PageModel
{
    [BindProperty]
    [Required(ErrorMessage = "Имя обязательно")]
    public string Name { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Неверный формат email")]
    public string Email { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Тема обязательна")]
    public string Subject { get; set; } 

    [BindProperty]
    [Required(ErrorMessage = "Сообщение обязательно")]
    public string Message { get; set; } 

    public void OnGet() { }

    [ValidateAntiForgeryToken]
    public IActionResult  OnPost()
    {
        var newContact = new User {Name = this.Name, Email = this.Email, Subject = this.Subject, Message = this.Message};
        // Валидация атрибутами
        if (!TryValidateModel(newContact))
        {
            return Page();
        }

        // Доп. валидация: email должен заканчиваться на .edu
        if (!Email.EndsWith(".edu", StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError("Email", "Email должен оканчиваться на .edu");
            return Page();
        }

        SaveToCsv(newContact);
        return Page();
    }

    private void SaveToCsv(User user)
    {
        // Абсолютный путь к файлу
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users.csv");
        Console.WriteLine($"Путь к файлу: {filePath}");

        // Создаем директорию, если ее нет
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        var records = new List<User>();

        if (System.IO.File.Exists(filePath))
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    records = csv.GetRecords<User>().ToList();
                }
            }
            catch
            {
                // Если файл поврежден, начинаем новый
                records = new List<User>();
            }
        }

        records.Add(user);

        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
    }
}

public class User
{
    public string Name { get; set; } 

    public string Email { get; set; }

    public string Subject { get; set; } 

    public string Message { get; set; } 
}
