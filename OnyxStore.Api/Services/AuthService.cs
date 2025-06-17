using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnyxStore.Api.Common;
using OnyxStore.Api.Data;
using OnyxStore.Api.Dtos;
using OnyxStore.Api.Interfaces;
using OnyxStore.Api.Models;
using OnyxStore.Api.Results;

namespace OnyxStore.Api.Services;

public class AuthService(AppDbContext dbContext, IPasswordHasher<User> passwordHasher)
    : IAuthService
{
    public async Task<AuthResult> RegisterAsync(PostUserDto dto)
    {
        var authResult = new AuthResult();
        if (dbContext.Users.Any(u => u.Email == dto.Email))
        {
            authResult.Status = AuthStatusResult.EmailAlreadyInUse;
            return authResult;
        }
        var user = new User
        {
            Email = dto.Email,
            Password = passwordHasher.HashPassword(null, dto.Password)
        };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        authResult.Status = AuthStatusResult.Success;
        authResult.Token = GenerateJwtToken(user);
        
        return authResult;
    }

    public async Task<AuthResult> LoginAsync(LoginUserDto dto)
    {
        var authResult = new AuthResult() {Status = AuthStatusResult.InvalidEmailOrPassword};
        
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
        if (user == null)
            return authResult;
        
        var result = passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if (result == PasswordVerificationResult.Failed)
            return authResult;
        
        authResult.Status = AuthStatusResult.Success;
        authResult.Token = GenerateJwtToken(user);
        
        return authResult;
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Aud, Settings.Audience)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.JwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: Settings.Issuer,
            audience: Settings.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}