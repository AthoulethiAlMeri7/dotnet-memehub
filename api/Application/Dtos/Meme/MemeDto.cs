namespace api.Application.Dtos
{
    public class MemeDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public Guid UserId { get; set; }
        public Guid TemplateId { get; set; }
        public List<TextBlockDto> TextBlocks { get; set; } = new();
    }
}