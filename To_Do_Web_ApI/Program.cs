using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using To_Do_Web_ApI.Auth.Service;
using To_Do_Web_ApI.Data;
using To_Do_Web_ApI.Users.Service;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

try
{
    // This options just a blueprint of my db context but without any additional details
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString))
    );
    Console.WriteLine("✅ Connected to the database");
}
catch (Exception e)
{
    Console.WriteLine("Failed to connect to the database");
    Console.WriteLine(e);
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
        };
    });
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddControllers();


var app = builder.Build();

await app.MigrateDbAsync();

app.UseAuthentication();
app.MapControllers();

app.Run();