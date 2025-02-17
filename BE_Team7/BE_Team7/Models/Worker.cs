namespace BE_Team7.Models
{
    public class Worker
    {
        public int WorkerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
