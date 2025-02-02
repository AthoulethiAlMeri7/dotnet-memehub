namespace api.Application.Dtos
{
    public class CreateMemeDto
    {
        public string? Title { get; set; }

        public string ImageUrl { get; set; } = null!;

        public Guid TemplateId { get; set; }

        // public List<CreateTextBlockDto> TextBlocks { get; set; } = new();
    }
}