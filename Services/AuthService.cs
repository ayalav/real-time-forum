using RealTimeForum.Data;
using RealTimeForum.Data.Entities;
using BCrypt.Net;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RealTimeForum.Models.Enums;
using RealTimeForum.Models;
using Microsoft.EntityFrameworkCore;

namespace RealTimeForum.Services;

public interface IAuthService
{
    Task<LoginResult> Login(LoginDTO model);
    Task<LoginRes> Register(RegisterDTO model);
}

public class AuthService : IAuthService
{
    private readonly MyDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthService(MyDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    // Login a user and verify the password
    public async Task<LoginResult> Login(LoginDTO model)

    {
    var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == model.Username);

    if (user == null)
    {
        return new LoginResult { Result = LoginRes.UserNameIncorrect };
    }

    bool passwordMatch = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

    if (!passwordMatch)
    {
        return new LoginResult { Result = LoginRes.PasswordIncorrect };
    }

    var token = GenerateJwtToken(user);

    return new LoginResult { Result = LoginRes.Success, Token = token };
    }

    // Register a new user and hash the password with BCrypt
    public async Task<LoginRes> Register(RegisterDTO model)
    {
        if (await _dbContext.Users.AnyAsync(u => u.UserName == model.Username))
            return LoginRes.UserExist;

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var newUser = new User
        {
            UserName = model.Username,
            Password = hashedPassword,
        };

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync();

        return LoginRes.Success;
    }

    // Generate JWT token for authenticated user
    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("UserId", user.Id.ToString())
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


