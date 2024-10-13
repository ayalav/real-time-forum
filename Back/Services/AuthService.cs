using RealTimeForum.Data;
using RealTimeForum.Data.Entities;
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
    Task<LoginResponseDTO> Login(LoginRequestDTO model);
    Task<RegisterResult> Register(RegisterDTO model);
}

public class AuthService(MyDbContext dbContext, IConfiguration configuration) : IAuthService
{
    // Login a user and verify the password
    public async Task<LoginResponseDTO> Login(LoginRequestDTO model)
    {
        var user = await dbContext
            .Users
            .FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null)
            return new LoginResponseDTO { Result = LoginRes.UserNameIncorrect };

        bool passwordMatch = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

        if (!passwordMatch)
            return new LoginResponseDTO { Result = LoginRes.PasswordIncorrect };

        var token = GenerateJwtToken(user);

        return new LoginResponseDTO { Result = LoginRes.Success, Token = token };
    }

    // Register a new user and hash the password with BCrypt
    public async Task<RegisterResult> Register(RegisterDTO model)
    {
        if (model.Username == null)
            return RegisterResult.UserNameIncorrect;

        if (model.Password == null)
            return RegisterResult.PasswordIncorrect;

        var UserExist = await dbContext
            .Users
            .AnyAsync(u => u.UserName == model.Username);

        if (UserExist)
            return RegisterResult.UserExist;

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var newUser = new User
        {
            Email = model.Email,
            UserName = model.Username,
            Password = hashedPassword,
        };

        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync();

        return RegisterResult.Success;
    }

    // Generate JWT token for authenticated user
    private string GenerateJwtToken(User user)
    {
        Claim[] claims = [
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", user.Id.ToString())
        ];

        var jwtKey = configuration["Jwt:Key"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}


