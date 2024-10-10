using RealTimeForum.Data;
using RealTimeForum.Data.Entities;
using BCrypt.Net;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RealTimeForum.Models.Enums;
using RealTimeForum.Models;



namespace RealTimeForum.Services;

public class AuthService
{
    private readonly MyDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public AuthService(MyDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    // Login a user and verify the password
    public string Login(LoginDTO model, ref LoginRes loginResult)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.UserName == model.Username);

        if (user == null)
        {
            loginResult = LoginRes.UserNameIncorrect;
            return null;
        }

        // Verify the password using BCrypt
        bool passwordMatch = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

        if (!passwordMatch)
        {
            loginResult = LoginRes.PasswordIncorrect;
            return null;
        }

        // Create JWT token
        var token = GenerateJwtToken(user);

        loginResult = LoginRes.Success;
        return token;
    }

    // Register a new user and hash the password with BCrypt
    public LoginRes Register(RegisterDTO model)
    {
        if (_dbContext.Users.Any(u => u.UserName == model.Username))
            return LoginRes.UserExist;

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var newUser = new User
        {
            UserName = model.Username,
            Password = hashedPassword,
        };

        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();

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
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


