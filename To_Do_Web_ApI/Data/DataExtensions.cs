using Microsoft.EntityFrameworkCore;

namespace To_Do_Web_ApI.Data;

public static class DataExtensions
{
    public static void MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext  = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
         dbContext.Database.MigrateAsync();
    }
}