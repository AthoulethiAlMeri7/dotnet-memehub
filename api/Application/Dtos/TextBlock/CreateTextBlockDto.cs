using System.ComponentModel.DataAnnotations;

public class CreateTextBlockDto
{
    public string Text { get; set; } = null!;
    public int X { get; set; }
    public int Y { get; set; }
    public string? FontSize { get; set; }
    public Guid MemeId { get; set; }
}
