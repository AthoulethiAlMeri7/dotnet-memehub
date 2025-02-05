namespace api.Application.Dtos.UserDtos
{
    public class ReturnedUserDto
    {
        public required Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string ProfilePic { get; set; }
        public required List<string> Roles { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}