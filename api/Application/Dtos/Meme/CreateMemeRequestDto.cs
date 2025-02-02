namespace api.Application.Dtos
{
    public class CreateMemeRequestDto
    {
        public required CreateMemeDto Meme { get; set; }
        public required CreateTextBlockDto TextBlock { get; set; }

    }
}