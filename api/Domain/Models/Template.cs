namespace API.Domain.Models
{
    public class Template
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public required string ImageUrl { get; set; }
        public Byte[]? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual ICollection<Meme>? Memes { get; set; }

        public Template()
        {
            Memes = new List<Meme>();
        }

        public void OnPersist()
        {
            // Implementation for actions on persist
        }

        public void PreSoftDelete()
        {
            // Implementation for soft deleting template's memes
        }
    }
}