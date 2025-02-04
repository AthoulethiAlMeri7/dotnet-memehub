namespace api.Application.Dtos.UserDtos
{
    public class UpdateUserDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}