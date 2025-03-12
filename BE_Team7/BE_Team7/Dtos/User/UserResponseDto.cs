namespace BE_Team7.Dtos.User
{
    public class UserResponseDto
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public IList<string>? Roles { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
