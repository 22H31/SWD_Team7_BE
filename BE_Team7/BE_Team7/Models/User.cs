using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? SkinType { get; set; }
    public string? Address { get; set; }
    public string? Avartar { get; set; }
    public DateTime CreatedAt { get; set; }
}
