using Microsoft.AspNetCore.Identity;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public virtual required ICollection<Meme> Memes { get; set; }

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
      DeletedAt = DateTime.UtcNow;
    }


    public void OnUpdate()
    {
      UpdatedAt = DateTime.UtcNow;
    }
    public void OnPersist()
    {
      CreatedAt = DateTime.UtcNow;
      UpdatedAt = DateTime.UtcNow;
      if (string.IsNullOrEmpty(ProfilePic))
      {
        ProfilePic = "/Infrastructure/Assets/ProfilePics/default.png";
      }
    }
  }
}