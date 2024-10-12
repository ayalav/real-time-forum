using RealTimeForum.Models.Enums;

namespace RealTimeForum.Models;

public class LoginDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginResult
{
    public LoginRes Result { get; set; }
    public string Token { get; set; }
}

