using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
    public class LoginResponseDto
    {
        public string Id { get; set; }
        public bool IsLogedIn { get; set; } = false;
        public required string JwtToken { get; set; }
        public string? Email { get; set; }
        public IList<string>? Roles { get; set; }
        public string? Name { get; set; }
        public string? SkinType { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}