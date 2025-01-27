using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Models
{
  public class Meme : BaseEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(125)]
    public string? Title { get; set; }

    [Required]
    [Url]
    public required string ImageUrl { get; set; }

    [Required]
    public required Guid UserId { get; set; }

    [Required]
    public required Guid TemplateId { get; set; }

    [ForeignKey("UserId")]
    public virtual required ApplicationUser User { get; set; }

    [ForeignKey("TemplateId")]
    public virtual required Template Template { get; set; }

    [InverseProperty("Meme")]
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