
public record TelegramUpdates(bool Ok, List<TelegramUpdate> Result);
public record TelegramUpdate(TelegramMessage Message);
public record TelegramMessage(TelegramUser From, TelegramChat Chat);
public record TelegramChat(long Id);
public record TelegramUser(string Username);