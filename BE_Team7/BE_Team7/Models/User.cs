using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class User : IdentityUser
{
   public required string Name { get; set; }

    public string? SkinType { get; set; }

    public DateTime CreatedAt { get; set; }
}
