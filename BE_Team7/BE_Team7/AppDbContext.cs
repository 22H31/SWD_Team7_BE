namespace BE_Team7;

using BE_Team7.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> User { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<CategoryTitle> CategoryTitle { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Blog> Blog { get; set; }
    public DbSet<Cart> Cart { get; set; }
    public DbSet<CartItem> CartItem { get; set; }
    public DbSet<Feedback> Feedback { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderDetail> OrderDetail { get; set; }
    public DbSet<Payment> Payment { get; set; }
    public DbSet<Promotion> Promotion { get; set; }
    public DbSet<Brand> Brand { get; set; }
    public DbSet<SkinCareRoutine> SkinCareRoutine { get; set; }
    public DbSet<SkinTest> SkinTest { get; set; }
    public DbSet<SuggestProducts> SuggestProducts { get; set; }
    public DbSet<ProductImage> ProductImage { get; set; }
    public DbSet<ProductVariant> ProductVariant { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole{
                    Name="Admin",
                    NormalizedName="ADMIN"
                },
                new IdentityRole
                {
                    Name="StaffSale",
                    NormalizedName="STAFFSALE"
                },
                new IdentityRole
                {
                    Name="Staff",
                    NormalizedName="STAFF"
                },
                new IdentityRole{
                    Name="User",
                    NormalizedName="USER"
                }
            };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}
