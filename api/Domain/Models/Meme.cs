using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    public required Guid UserId { get; set; }

    [Required]
    public required Guid TemplateId { get; set; }

    [ForeignKey("UserId")]
    public virtual ApplicationUser? User { get; set; }

    [ForeignKey("TemplateId")]
    public virtual Template? Template { get; set; }

    [InverseProperty("Meme")]
    public virtual ICollection<TextBlock> TextBlocks { get; set; }

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