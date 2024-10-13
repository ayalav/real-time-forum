using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeForum.Models;
using RealTimeForum.Models.Enums;
using RealTimeForum.Services;

namespace RealTimeForum.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    // Registration endpoint
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerModel)
    {
        var result = await authService.Register(registerModel);

        return result switch
        {
            RegisterResult.UserExist => BadRequest("Username already exists."),
            RegisterResult.UserNameIncorrect => BadRequest("Username is required."),
            RegisterResult.PasswordIncorrect => BadRequest("Password is required."),
            RegisterResult.Success => Ok(result),
            _ => BadRequest("An error occurred.")
        };
    }

    // Login endpoint
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginModel)
    {
        var loginResult = await authService.Login(loginModel);

        return loginResult.Result switch
        {
            LoginRes.UserNameIncorrect or LoginRes.PasswordIncorrect => Unauthorized("Invalid login credentials."),
            LoginRes.Success => Ok(new { token = loginResult.Token }),
            _ => Unauthorized("An error occurred.")
        };
    }
}

