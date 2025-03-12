namespace BE_Team7.Dtos.User
{
    public class UserDetailDto
    {
        public string? Avatar { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SkinType { get; set; }
        public string? Address { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
