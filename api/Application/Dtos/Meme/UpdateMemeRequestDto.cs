namespace api.Application.Dtos
{
    public class UpdateMemeRequestDto
    {
        public required UpdateMemeDto Meme { get; set; }
        public required List<CreateTextBlockDto> TextBlocks { get; set; }
    }
}