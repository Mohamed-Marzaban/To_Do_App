namespace To_Do_Web_ApI.Data;
using Microsoft.EntityFrameworkCore;
using Model.Entity;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options){}

    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> TaskItems =>Set<TaskItem>();

}