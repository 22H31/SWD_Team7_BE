namespace BE_Team7;
using BE_Team7.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> User { get; set; }
    public DbSet<Worker> Worker { get; set; }   
    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole{
                    Name="Admin",
                    NormalizedName="ADMIN"
                },
                new IdentityRole{
                    Name="User",
                    NormalizedName="USER"
                }
            };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}
