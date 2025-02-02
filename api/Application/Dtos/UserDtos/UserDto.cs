namespace api.Application.Dtos.UserDtos{
    public class UserDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}