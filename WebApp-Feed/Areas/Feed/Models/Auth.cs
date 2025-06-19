using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApp_Feed.Models;

public partial class Auth : IdentityUser<long>
{
    public long UserId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public byte[]? LastLogin { get; set; }

    public string? ResetToken { get; set; }

    public byte[]? TokenExpiry { get; set; }

    public virtual User User { get; set; } = null!;
}
