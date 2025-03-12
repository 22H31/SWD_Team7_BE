namespace BE_Team7.Dtos.User
{
    public class UpdateUserRequestDto
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
