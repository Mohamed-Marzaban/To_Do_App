using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using To_Do_Web_ApI.Model.Entity;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace To_Do_Web_ApI.Auth.Service;

public class AuthService
{
    private readonly IConfiguration _configuration;
    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateJwtToken(User user)
    {
        Console.WriteLine(user.username);
        var jwtSection = _configuration.GetSection("Jwt");
        var jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(jwtKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim("username", user.username),
            new Claim("userId",user.Id.ToString()),
        };

        // Creates an instance of jwt token structure 
        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires:DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSection["ExpireMinutes"])),
            signingCredentials: creds
        );
       
        //Creates the jwt token and convert to string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}