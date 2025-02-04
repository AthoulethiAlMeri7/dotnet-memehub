namespace API.Application.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string ProfilePic { get; set; }
        public List<string>? Roles { get; set; }
    }
}
