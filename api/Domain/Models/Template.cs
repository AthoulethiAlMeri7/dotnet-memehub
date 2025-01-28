using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Models
{
    public class Template : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(125)]
        public string? Title { get; set; }

        [Required]
        public required string ImageUrl { get; set; }

        [InverseProperty("Template")]
        public virtual ICollection<Meme>? Memes { get; set; }

        public Template()
        {
            Memes = new List<Meme>();
        }

        // public void OnPersist()
        // {
        //     // Implementation for actions on persist
        // }

        // public void PreSoftDelete()
        // {
        //     // Implementation for soft deleting template's memes
        // }
    }
}