using RealTimeForum.Models.Enums;

namespace RealTimeForum.Models;

public class LoginRequestDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginResponseDTO
{
    public LoginRes Result { get; set; }
    public string? Token { get; set; }
}

