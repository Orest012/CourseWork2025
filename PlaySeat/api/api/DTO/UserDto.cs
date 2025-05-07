namespace api.DTO
{
    public class UserDto
    {
        public long UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateOnly CreatedAt { get; set; }
    }
}
