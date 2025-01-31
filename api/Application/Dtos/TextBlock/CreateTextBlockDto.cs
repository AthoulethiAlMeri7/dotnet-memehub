using System.ComponentModel.DataAnnotations;

public class CreateTextBlockDto
{
    [Required]
    public string Text { get; set; } = null!;

    [Required]
    public int X { get; set; }

    [Required]
    public int Y { get; set; }

    [MaxLength(10)]
    public string? FontSize { get; set; }

    [Required]
    public Guid MemeId { get; set; }
}
