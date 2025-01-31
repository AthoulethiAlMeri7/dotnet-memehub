using System.ComponentModel.DataAnnotations;

public class UpdateMemeDto
{
    [MaxLength(125)]
    public string? Title { get; set; }

    [Url]
    public string? ImageUrl { get; set; }

    public List<UpdateTextBlockDto>? TextBlocks { get; set; }
}
