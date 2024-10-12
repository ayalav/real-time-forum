using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeForum.Models;
using RealTimeForum.Models.Enums;
using RealTimeForum.Services;

namespace RealTimeForum.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // Registration endpoint
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerModel)
    {
        var result = await _authService.Register(registerModel);

        if (result == LoginRes.UserExist)
        {
            return BadRequest("Username already exists.");
        }

        return Ok(result);

    }

    // Login endpoint
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginModel)
    {
        var loginResult = await _authService.Login(loginModel);

        if (loginResult.Result == LoginRes.UserNameIncorrect || loginResult.Result == LoginRes.PasswordIncorrect)
        {
            return Unauthorized("Invalid login credentials.");
        }

        return Ok(new { token = loginResult.Token });
    }
}

