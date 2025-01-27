namespace API.Domain.Models
{
  public class Meme : BaseEntity
  {
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public required string ImageUrl { get; set; }
    public required Guid UserId { get; set; }
    public required Guid TemplateId { get; set; }
    public virtual required ApplicationUser User { get; set; }
    public virtual required Template Template { get; set; }
    public virtual required ICollection<TextBlock> TextBlocks { get; set; }

    public Meme()
    {
      TextBlocks = new List<TextBlock>();
    }

    // public void OnPersist()
    // {
    //   // use pareent class onPersist method


    // }

    // public void PreSoftDelete()
    // {
    //   // Implementation for soft deleting meme's text blocks
    // }

  }
}