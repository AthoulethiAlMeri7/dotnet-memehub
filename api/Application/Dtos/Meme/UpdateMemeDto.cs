namespace api.Application.Dtos
{
    public class UpdateMemeDto
    {
        public string? Title { get; set; }
        public List<UpdateTextBlockDto>? TextBlocks { get; set; }
    }
}