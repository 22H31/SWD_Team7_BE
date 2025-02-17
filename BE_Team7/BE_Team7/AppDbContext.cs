namespace BE_Team7;
using BE_Team7.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> User { get; set; }
    public DbSet<Worker> Worker { get; set; }
    
}
