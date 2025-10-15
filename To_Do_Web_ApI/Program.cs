using Microsoft.EntityFrameworkCore;
using To_Do_Web_ApI.Data;
using To_Do_Web_ApI.Users.Service;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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

builder.Services.AddScoped<UserService>();
builder.Services.AddControllers();

var app = builder.Build();

await app.MigrateDbAsync();

app.Run();