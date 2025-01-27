namespace API.Domain.Models
{
    public class TextBlock
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string? FontSize { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid MemeId { get; set; }
        public virtual required Meme Meme { get; set; }

        public void OnPersist()
        {
            // Implementation for actions on persist
        }

        public void PreSoftDelete()
        {
            // Implementation for soft deleting text block
        }
    }
}