using Chirp.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Chirp.Infrastructure;


public class CheepDbContext : IdentityDbContext<Author>
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Recheep> Recheeps { get; set; }
    public DbSet<Follows> Follows { get; set; }
    
    public CheepDbContext(DbContextOptions<CheepDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Author>()
            .HasMany(a => a.Cheeps)
            .WithOne(c => c.Author)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.Entity<Follows>()
            .HasKey(f => new { f.FollowsId, f.FollowedById });

        builder.Entity<Recheep>()
            .HasKey(r => new { r.AuthorID, r.CheepID });

        builder.Entity<Follows>()
            .HasOne(f => f.FollowsAuthor)
            .WithMany(a => a.Following)
            .HasForeignKey(f => f.FollowsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Follows>()
            .HasOne(f => f.FollowedByAuthor)
            .WithMany(a => a.FollowedBy)
            .HasForeignKey(f => f.FollowedById)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
