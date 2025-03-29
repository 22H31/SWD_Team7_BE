using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using BE_Team7.Models;

public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public virtual ICollection<SkinTestRerult> RerultSkinTest { get; set; } = new List<SkinTestRerult>();
    public virtual ICollection<AvatarImage> AvatarImages { get; set; } = new List<AvatarImage>();
    public DateTime CreatedAt { get; set; }
}
