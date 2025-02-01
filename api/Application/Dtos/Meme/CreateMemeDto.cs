using System.ComponentModel.DataAnnotations;

public class CreateMemeDto
{
    [MaxLength(125)]
    public string? Title { get; set; }

    [Required]
    [Url]
    public string ImageUrl { get; set; } = null!;

    [Required]
    public Guid TemplateId { get; set; }

    public List<CreateTextBlockDto> TextBlocks { get; set; } = new();
}
