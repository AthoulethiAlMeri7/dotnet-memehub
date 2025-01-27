using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Models
{
    public class TextBlock : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public required string Text { get; set; }

        [Required]
        public int x { get; set; }

        [Required]
        public int y { get; set; }

        [MaxLength(10)]
        public string? FontSize { get; set; }

        [Required]
        public Guid MemeId { get; set; }

        [ForeignKey("MemeId")]
        public virtual required Meme Meme { get; set; }

        // public void OnPersist()
        // {
        //     // Implementation for actions on persist
        // }

        // public void PreSoftDelete()
        // {
        //     // Implementation for soft deleting text block
        // }
    }
}