namespace BE_Team7.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SkinType { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? RoutineId { get; set; }  // Có thể null nếu không có RoutineId
    }
}
