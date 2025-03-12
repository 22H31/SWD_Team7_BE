using System.ComponentModel.DataAnnotations;

namespace BE_Team7.Dtos.User
{
    public class CreateUserRepositoryDto
    {
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
