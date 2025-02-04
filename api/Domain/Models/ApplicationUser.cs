using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API.Domain.Models
{
  public class ApplicationUser : IdentityUser<Guid>
  {
    [Required]
    public DateTime CreatedAt { get; set; }

    public string? ProfilePic { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [InverseProperty("User")]
    [JsonIgnore]
    public virtual ICollection<Meme> Memes { get; set; }

    [NotMapped]
    public List<string> Roles { get; set; } = new List<string>();

    public ApplicationUser()
    {
      Memes = new List<Meme>();
    }

    // public ApplicationUser AddMeme(Meme meme)
    // {
    //     // Implementation for adding a meme
    //     return this;
    // }

    // public ApplicationUser RemoveMeme(Meme meme)
    // {
    //     // Implementation for removing a meme
    //     return this;
    // }

    public void PreSoftDelete()
    {
      IsDeleted = true;
      DeletedAt = DateTime.Now;
    }

    public void OnUpdate()
    {
      UpdatedAt = DateTime.Now;
    }
    public void OnPersist()
    {
      IsDeleted = false;
      CreatedAt = DateTime.Now;
      UpdatedAt = DateTime.Now;
      if (string.IsNullOrEmpty(ProfilePic))
      {
        ProfilePic = "/Infrastructure/Assets/ProfilePics/default.png";
      }
    }
  }
}