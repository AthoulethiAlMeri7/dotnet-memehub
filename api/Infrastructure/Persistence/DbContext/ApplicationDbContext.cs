using API.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API.Infrastructure.Persistence.DbContext
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Meme> Memes { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Template> Templates { get; set; }
    public DbSet<TextBlock> TextBlocks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<Meme>()
                .HasOne(m => m.User)
                .WithMany(u => u.Memes)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<Meme>()
          .HasOne(m => m.Template)
          .WithMany(t => t.Memes)
          .HasForeignKey(m => m.TemplateId)
          .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<TextBlock>()
          .HasOne(tb => tb.Meme)
          .WithMany(m => m.TextBlocks)
          .HasForeignKey(tb => tb.MemeId)
          .OnDelete(DeleteBehavior.Cascade);
    }

    public override int SaveChanges()
    {

      foreach (var entry in ChangeTracker.Entries<BaseEntity>())
      {
        if (entry.State == EntityState.Deleted)
        {
          entry.State = EntityState.Modified;
          entry.Entity.PreSoftDelete();
        }
        else if (entry.State == EntityState.Added)
        {
          entry.Entity.OnPersist();
        }
        else if (entry.State == EntityState.Modified)
        {
          entry.Entity.OnUpdate();
        }
      }

      return base.SaveChanges();
    }
  }
}