using System.ComponentModel.DataAnnotations.Schema;

namespace API.Domain.Models
{
  [NotMapped]
  public abstract class BaseEntity
  {
    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public void PreSoftDelete()
    {
      IsDeleted = true;
      DeletedAt = DateTime.Now;
    }

    public void OnPersist()
    {
      if (CreatedAt == null)
      {
        CreatedAt = DateTime.Now;
        Console.WriteLine($"Current Date and Time: {CreatedAt}");

      }
      UpdatedAt = DateTime.Now;
      IsDeleted = false;
    }

    public void OnUpdate()
    {
      UpdatedAt = DateTime.Now;
    }
  }
}