using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using BE_Team7.Models;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? SkinType { get; set; }
    public string? Address { get; set; }
    public AvatarImage? Avatar { get; set; }
    public DateTime CreatedAt { get; set; }
}
