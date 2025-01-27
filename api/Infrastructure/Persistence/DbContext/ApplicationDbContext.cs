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

    }
  }
}