using System.ComponentModel.DataAnnotations;

public class UpdateTextBlockDto
{
    public string? Text { get; set; }
    public int? X { get; set; }
    public int? Y { get; set; }

    [MaxLength(10)]
    public string? FontSize { get; set; }
}
