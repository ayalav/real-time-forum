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
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    // Registration endpoint
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDTO registerModel)
    {
        var result = _authService.Register(registerModel);

        if (result == LoginRes.UserExist)
        {
            return BadRequest("Username already exists.");
        }

        return Ok(result);
    }

    // Login endpoint
    [HttpPost("login")]
 public IActionResult Login([FromBody] LoginDTO loginModel)
{
    LoginRes loginResult = LoginRes.Success;

    var token = _authService.Login(loginModel, ref loginResult);

    if (loginResult == LoginRes.UserNameIncorrect)
    {
        return Unauthorized("Username is incorrect.");
    }

    if (loginResult == LoginRes.PasswordIncorrect)
    {
        return Unauthorized("Password is incorrect.");
    }

    // Return the JWT token in the response
    return Ok(new { token = token });
}
}

