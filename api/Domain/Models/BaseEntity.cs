using System;

namespace API.Domain.Models
{
  public abstract class BaseEntity
  {
    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public void PreSoftDelete()
    {
      IsDeleted = true;
      DeletedAt = DateTime.UtcNow;
    }

    public void OnPersist()
    {
      if (CreatedAt == null)
      {
        CreatedAt = DateTime.UtcNow;
      }
      UpdatedAt = DateTime.UtcNow;
    }

    public void OnUpdate()
    {
      UpdatedAt = DateTime.UtcNow;
    }
  }
}